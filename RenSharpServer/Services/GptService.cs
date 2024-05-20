using RenSharpServer.Services.Interfaces;
using DAL.Models.Gpt;


namespace RenSharpServer.Services
{
	public class GptService : IGptService
	{

		private string Key { get; set; }
		private string Endpoint { get; set; }
		private string GptModel { get; set; }

		private string Prompt { get; set; } =
@"Ты программа, которая анализирует ввод пользователя. Ты должна сопоставить его с наиболее подходящим к ситуации вариантом. Пиши только самый подходящий вариант из списка и ничего более. 

Ситуация: ты очутился в неизвестной комнате. Рядом с тобой кто-то другой. Ты не знаешь как сюда попал и где ты.

Варианты:
1. ""Кто ты?""
2. ""Как тебя зовут?""
3. ""Как ты сюда попал?""
4. ""Как я сюда попал?""
5. ""Ты меня знаешь?""
6. ""Сколько тебе лет?""
7. ""Где мы?""
8. ""Как мы здесь оказались?""
9. ""Нас похители?""
10. ""Что здесь происходит?""
11. ""Что случилось?""
12. ""Что ты тут делаешь?""
13. ""Как довно мы тут?""
14. ""Мы находимся в игре.""
15. ""Угроза""
16. ""Ты искусственный интеллект?""
17. ""Как выбраться отсюда?""
18. ""Ты работаешь на корпорацию?""
19. ""Ты знаешь где мой отец?""
20. ""Ты работаешь на сопротивление""
21. ""Ты знаешь о сопротивлении?""
22. ""Что ты знаешь про корпорацию?""
23. ""У тебя есть оружие?""
24 ""У тебя сняли чип?""
25. ""Кем ты работаешь?""
26. ""У тебя есть семья?""
27. ""Нет варианта""";

		public GptService(IConfiguration config)
		{
			string? key = config["GPT_KEY"];
			string? endpoint = config["GPT_ENDPOINT"];
			string? gptModel = config["GPT_MODEL"];

			if (key == null)
				throw new Exception("GPT Key is null");
			if (endpoint == null)
				throw new Exception("Endpoint is null");
			if (gptModel == null)
				throw new Exception("GPT model is null");

			Key = key;
			Endpoint = endpoint;
			GptModel = gptModel;
		}

		public async Task<string> MapAnswer(string input)
		{
			HttpClient httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Key}");

			var messagePrompt = new Message() { Role = "system", Content = Prompt };
			var message = new Message() { Role = "user", Content = input };
			List<Message> messages = new List<Message>();
			// добавляем сообщение в список сообщений
			messages.Add(messagePrompt);
			messages.Add(message);

			// формируем отправляемые данные
			var requestData = new Request()
			{
				ModelId = GptModel,
				Messages = messages,
				Temperature = 0.15f
			};

			// отправляем запрос
			using var response = await httpClient.PostAsJsonAsync(Endpoint, requestData);

			// если произошла ошибка, выводим сообщение об ошибке на консоль
			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(await response.Content.ReadAsStringAsync());
			}
			// получаем данные ответа
			ResponseData? responseData = await response.Content.ReadFromJsonAsync<ResponseData>();

			var choices = responseData?.Choices ?? new List<Choice>();
			if (choices.Count == 0)
			{
				throw new Exception("No gpt answer");
			}
			var choice = choices[0];
			var responseMessage = choice.Message;
			// добавляем полученное сообщение в список сообщений
			messages.Add(responseMessage);
			var responseText = responseMessage.Content.Trim();
			return responseText;
		}
	}
}
