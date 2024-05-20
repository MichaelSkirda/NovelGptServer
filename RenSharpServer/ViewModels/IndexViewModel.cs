using DAL.Models;

namespace RenSharpServer.ViewModels
{
    public class IndexViewModel
	{
		public List<Dialog>? Dialogs { get; set; }
		public bool CurrentStatus { get; set; }
		public IndexViewModel(List<Dialog>? dialogs, bool currentStatus)
		{
			Dialogs = dialogs;
			CurrentStatus = currentStatus;
		}
	}
}
