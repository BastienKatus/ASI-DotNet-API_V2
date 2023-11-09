using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASI_DotNet_API_V2.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASI_Dotnet_API_V2.Model.EntityFramework;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using ASI_DotNet_API_V2.Model.DataManager;
using ASI_DotNet_API_V2.Model.Repository;
using NuGet.ContentModel;

namespace ASI_DotNet_API_V2.Controllers.Tests
{
    [TestClass()]
    public class UtilisateursControllerTests
    {
        private UtilisateursController _controller;
        private ASIDBContext _context;
        private IDataRepository<Utilisateur> _dataRepository;
        public UtilisateursControllerTests()
        {
            var builder = new DbContextOptionsBuilder().UseNpgsql("Server=localhost;port=5432;Database=ASIDB; uid=postgres; password = postgres; ");
            _context = new ASIDBContext(builder.Options);
            _dataRepository = new UtilisateurManager(_context);
            _controller = new UtilisateursController(_dataRepository);
        }

        [TestMethod]
        public void GetAll()
        {
            var users = _context.Utilisateurs.ToList();

            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]
        public void GetById_OK()
        {
            int userIdToRetrieve = 42;

            var user = _context.Utilisateurs.Where(c => c.UtilisateurId == userIdToRetrieve).FirstOrDefault();

            Assert.IsNotNull(user);
            Assert.AreEqual(userIdToRetrieve, user.UtilisateurId);
        }

        [TestMethod]
        public void GetById_KO()
        {
            int userIdToRetrieve = -1;

            var user = _context.Utilisateurs.Where(c => c.UtilisateurId == userIdToRetrieve).FirstOrDefault();

            Assert.IsNotNull(user);
            Assert.AreEqual(userIdToRetrieve, user.UtilisateurId);
        }

        [TestMethod]
        public void GetByEmail_OK()
        {
            string userMailToRetrieve = "rmulryan4u@yale.edu";

            var user = _context.Utilisateurs.Where(c => c.Mail == userMailToRetrieve).FirstOrDefault();

            Assert.IsNotNull(user);
            Assert.AreEqual(userMailToRetrieve, user.Mail);
        }

        [TestMethod]
        public void GetByEmail_KO()
        {
            string userMailToRetrieve = "idontexistindb@cpomofote.org";

            var user = _context.Utilisateurs.Where(c => c.Mail == userMailToRetrieve).FirstOrDefault();

            Assert.IsNotNull(user);
            Assert.AreEqual(userMailToRetrieve, user.Mail);
        }

        [TestMethod]
        public void Postutilisateur_ModelValidated_CreationOK()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE du WS (=> la décommenter) ou la méthode remove du DbSet
             Utilisateur userAtester = new Utilisateur()
             {
                 Nom = "MACHIN",
                 Prenom = "Luc",
                 Mobile = "0606070809",
                 Mail = "machin" + chiffre + "@gmail.com",
                 Pwd = "Toto1234!",
                 Rue = "Chemin de Bellevue",
                 CodePostal = "74940",
                 Ville = "Annecy-le-Vieux",
                 Pays = "France",
                 Latitude = null,
                 Longitude = null
             };
            // Act
            var result = _controller.PostUtilisateur(userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d’attendre l’ajout
             // Assert
            Utilisateur? userRecupere = _context.Utilisateurs.Where(u => u.Mail.ToUpper() == userAtester.Mail.ToUpper()).FirstOrDefault(); // On récupère l'utilisateur créé directement dans la BD grace à son mail unique
            // On ne connait pas l'ID de l’utilisateur envoyé car numéro automatique.
            // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 users
            userAtester.UtilisateurId = userRecupere.UtilisateurId;
            Assert.AreEqual(userRecupere, userAtester, "Utilisateurs pas identiques");
        }

        [TestMethod()]
        public void DeleteUtilisateurTest()
        {
            Assert.Fail();
        }
    }
}