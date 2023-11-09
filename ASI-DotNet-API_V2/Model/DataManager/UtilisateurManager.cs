using ASI_Dotnet_API_V2.Model.EntityFramework;
using ASI_DotNet_API_V2.Model.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASI_DotNet_API_V2.Model.DataManager
{
    public class UtilisateurManager : IDataRepository<Utilisateur>
    {
        readonly ASIDBContext? AsiDbContext;
        public UtilisateurManager() { }
        public UtilisateurManager(ASIDBContext context)
        {
            AsiDbContext = context;
        }
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetAllAsync()
        {
            return await AsiDbContext.Utilisateurs.ToListAsync();
        }
        public async Task<ActionResult<Utilisateur>> GetByIdAsync(int id)
        {
            return await AsiDbContext.Utilisateurs.FirstOrDefaultAsync(u => u.UtilisateurId == id);
        }
        public async Task<ActionResult<Utilisateur>> GetByStringAsync(string mail)
        {
            return await AsiDbContext.Utilisateurs.FirstOrDefaultAsync(u => u.Mail.ToUpper() == mail.ToUpper());
        }
        public async Task AddAsync(Utilisateur entity)
        {
            await AsiDbContext.Utilisateurs.AddAsync(entity);
            await AsiDbContext.SaveChangesAsync();

        }

        public async Task UpdateAsync(Utilisateur utilisateur, Utilisateur entity)
        {
            AsiDbContext.Entry(utilisateur).State = EntityState.Modified;
            utilisateur.UtilisateurId = entity.UtilisateurId;
            utilisateur.Nom = entity.Nom;
            utilisateur.Prenom = entity.Prenom;
            utilisateur.Mail = entity.Mail;
            utilisateur.Rue = entity.Rue;
            utilisateur.CodePostal = entity.CodePostal;
            utilisateur.Ville = entity.Ville;
            utilisateur.Pays = entity.Pays;
            utilisateur.Latitude = entity.Latitude;
            utilisateur.Longitude = entity.Longitude;
            utilisateur.Pwd = entity.Pwd;
            utilisateur.Mobile = entity.Mobile;
            utilisateur.NotesUtilisateurs = entity.NotesUtilisateurs;
            await AsiDbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Utilisateur utilisateur)
        {
            AsiDbContext.Utilisateurs.Remove(utilisateur);
            await AsiDbContext.SaveChangesAsync();
        }
    }
}