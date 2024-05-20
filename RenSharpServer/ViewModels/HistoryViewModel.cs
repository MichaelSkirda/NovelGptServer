using DAL.Models;

namespace RenSharpServer.ViewModels
{
	public class HistoryViewModel
	{
		public List<Dialog>? Dialogs { get; set; }
		public HistoryViewModel(List<Dialog>? dialogs)
		{
			Dialogs = dialogs;
		}
	}
}
