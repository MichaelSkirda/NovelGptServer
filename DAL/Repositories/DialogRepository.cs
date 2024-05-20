using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class DialogRepository : IDialogRepository
    {
        private AppDbContext db { get; set; }

        public DialogRepository(AppDbContext dbContext)
        {
            db = dbContext;
        }

        public void Add(Dialog dialog)
        {
            db.Dialogs.Add(dialog);
            db.SaveChanges();
        }


        public List<Dialog> GetActiveDialogs()
        {
            return db.Dialogs
                .Where(x => x.Accepted == null)
                .Include(x => x.Messages)
                .ToList();
        }

        public Dialog? GetDialogOrDefault(int id)
        {
            return db.Dialogs
                .Where(x => x.Id == id)
                .Include(x => x.Messages)
                .FirstOrDefault();
        }

		public void SaveAnswer(int dialogId, string answer)
		{
            Dialog? dialog = db.Dialogs
                .First(x => x.Id == dialogId);
            dialog.Answer = answer;
            db.SaveChanges();
		}

		public void SaveChanges()
		{
            db.SaveChanges();
		}

		public void SetAccepted(int dialogId, string message)
		{
			Dialog? dialog = GetDialogOrDefault(dialogId);
            if (dialog == null)
                throw new ArgumentException("Not found");

            dialog.Accepted = message;
            db.SaveChanges();
		}

		public List<Dialog> GetAllDialogs()
		{
            return db.Dialogs.ToList();
		}

		public void DeleteAll()
		{
            // TODO DELETE
#if DEBUG
            db.Database.ExecuteSqlRaw("TRUNCATE TABLE \"Dialogs\" CASCADE");
#endif
		}
	}
}