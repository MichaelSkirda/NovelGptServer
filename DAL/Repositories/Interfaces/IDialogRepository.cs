using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IDialogRepository
    {
        public List<Dialog> GetActiveDialogs();
        public void Add(Dialog dialog);
        public void SaveAnswer(int dialogId, string answer);
        public void SetAccepted(int dialogId, string message);
        public Dialog? GetDialogOrDefault(int id);
        public void SaveChanges();
		List<Dialog> GetAllDialogs();
        public void DeleteAll();
	}
}
