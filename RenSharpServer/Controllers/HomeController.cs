using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RenSharpServer.Hubs;
using RenSharpServer.Services.Interfaces;
using RenSharpServer.ViewModels;
using System.Diagnostics;

namespace RenSharpServer.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private IDialogRepository DialogRepository { get; set; }
        private IGptService GptService { get; set; }
        private IHubContext<EventsHub> EventHub { get; set; }

        private static bool IsManualMode { get; set; } = true;

        public HomeController(IDialogRepository dialogRepository, IGptService gptService,
            IHubContext<EventsHub> eventHub)
        {
            DialogRepository = dialogRepository;
            GptService = gptService;
            EventHub = eventHub;
        }

        public IActionResult Index()
        {
            List<Dialog> dialogs = DialogRepository.GetActiveDialogs();
            var viewModel = new IndexViewModel(dialogs, IsManualMode);
            return View(viewModel);
        }

        [HttpGet("dialog/active")]
        public IActionResult GetActiveDialogsIds()
        {
            List<int> dialogsIds = DialogRepository
                .GetActiveDialogs()
                .Select(x => x.Id)
                .ToList();

            return Json(dialogsIds);
        }

        [HttpGet("dialog/view/{id}")]
        public IActionResult GetDialogView(int id)
        {
            Dialog? dialog = DialogRepository.GetDialogOrDefault(id);
            if (dialog == null)
                return NotFound();
            return PartialView("_Message", dialog);
        }

        [HttpPost("dialog/startParsing")]
        public async Task<IActionResult> ParseMessage([FromBody] DialogDto dialogDto)
        {
            Dialog dialog = new Dialog()
            {
                Messages = dialogDto.Messages,
                Location = dialogDto.Location,
                UserMessage = dialogDto.UserMessage
            };
            dialog.Answer = null;
            dialog.Accepted = null;

            DialogRepository.Add(dialog);

            if (IsManualMode == false)
                await GetGptAnswer(dialog.Id);
            else
                await EventHub.Clients.All.SendAsync("Create", dialog.Id);

			return Json(dialog.Id);
        }

        [HttpGet("dialog/getstatus/{dialogId}")]
        public IActionResult GetStatus(int dialogId)
        {
			Dialog? dialog = DialogRepository.GetDialogOrDefault(dialogId);

            if (dialog == null)
                return NotFound();

            return Json(dialog.Answer);
        }

        [HttpPost("dialog/setAnswer")]
        public async Task<IActionResult> SetAnswer(int dialogId, string message)
        {
			Dialog? dialog = DialogRepository.GetDialogOrDefault(dialogId);
            if (dialog == null)
                return NotFound();

            dialog.Answer = message;
            try
            {
				DialogRepository.SaveAnswer(dialog.Id, message);
			}
            catch
            {
                return NotFound();
            }

			await EventHub.Clients.All.SendAsync("Update", dialog.Id);
			return Ok(dialog);
        }

        [HttpGet("dialog/useGpt/{dialogId}")]
        public async Task<IActionResult> GetGptAnswer(int dialogId)
        {
            Dialog? dialog = DialogRepository.GetDialogOrDefault(dialogId);
            if (dialog == null)
                return NotFound();
            
            string? userMessage = dialog.UserMessage;
            if (userMessage == null)
                return BadRequest();

            string answer = await GptService.MapAnswer(userMessage);
            dialog.Answer = answer;
            DialogRepository.SaveChanges();
            return Json(answer);
        }

        [HttpGet("server/status")]
        public IActionResult CheckStatus()
        {
            return Ok("OK. Mode: " + IsManualMode);
        }

        [HttpGet("server/getmode")]
        public IActionResult GetServerMode()
        {
            return Ok(IsManualMode);
        }

        [HttpGet("server/changemode")]
        public IActionResult ChangeMode(bool mode)
        {
            IsManualMode = mode;
            return Ok("Success! Current mode is: " + IsManualMode);
        }

        [HttpGet("dialog/getMoreTime/{dialogId}")]
        public async Task<IActionResult> GetMoreTime(int dialogId)
        {
            Dialog? dialog = DialogRepository.GetDialogOrDefault(dialogId);
            if(dialog == null)
                return NotFound();

            if(dialog.Answer != null && dialog.Answer != "$WAIT_MORE")
                return BadRequest("На диалог уже был получен ответ: " + dialog.Answer);

            dialog.Answer = "$WAIT_MORE";
            DialogRepository.SaveChanges();
			await EventHub.Clients.All.SendAsync("Update", dialog.Id);
			return Ok(dialog.Answer);
        }


        [HttpPost("dialog/accept")]
        public async Task<IActionResult> Accept([FromBody] IdMessagePairDto dto)
        {
            try
            {
				DialogRepository.SetAccepted(dto.DialogId, dto.Message);
			}
            catch
            {
                return NotFound();
            }

			await EventHub.Clients.All.SendAsync("Delete", dto.DialogId);
			return Ok();
        }

        [HttpGet("/history")]
        public IActionResult History()
        {
            List<Dialog> dialogs = DialogRepository.GetAllDialogs();
            HistoryViewModel viewModel = new HistoryViewModel(dialogs);

            return View(viewModel);
        }

        [HttpGet("/dialog/deleteAll")]
        public async Task<IActionResult> DeleteAll()
        {
            DialogRepository.DeleteAll();
			await EventHub.Clients.All.SendAsync("DeleteAll");
			return Ok();
        }


		//[HttpGet("dialog/delete/{id}")]
  //      public IActionResult Delete(int id)
  //      {
		//	bool hasDeleted = DialogRepository.Delete(id);

  //          if (hasDeleted == false)
  //              return BadRequest("Dialog was not deleted");
  //          return Ok();
  //      }

  //      [HttpPost("dialog/add")]
  //      public IActionResult Add([FromBody] DialogDto dialog)
  //      {
  //          if (dialog == null)
  //              return BadRequest("Dialog is null!");
		//	DialogRepository.Add(dialog);
  //          return Ok(dialog);
  //      }

	}
}
