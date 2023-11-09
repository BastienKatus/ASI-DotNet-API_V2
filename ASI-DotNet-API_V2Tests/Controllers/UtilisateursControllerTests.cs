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
using Moq;
using System.Data;
using System.Reflection;

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
        public void GetUtilisateurById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.GetUtilisateurById(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(user, actionResult.Value as Utilisateur);
        }

        [TestMethod]
        public void GetUtilisateurById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.GetUtilisateurById(0).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
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
        public void GetUtilisateurByMail_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            string userMailToRetrieve = "clilleymd@last.fm";

            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByStringAsync(userMailToRetrieve).Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.GetUtilisateurByEmail(userMailToRetrieve).Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(user, actionResult.Value as Utilisateur);
        }

        [TestMethod]
        public void GetUtilisateurByMail_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            string userMailToRetrieve = "idontexistindb@cpomofote.org";

            // Act
            var actionResult = userController.GetUtilisateurByEmail(userMailToRetrieve).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
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

        [TestMethod]
        public void Postutilisateur_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            Utilisateur user = new Utilisateur
            {
                Nom = "POISSON",
                Prenom = "Pascal",
                Mobile = "1",
                Mail = "poisson@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act
            var actionResult = userController.PostUtilisateur(user).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Utilisateur>), "Pas un ActionResult<Utilisateur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Utilisateur), "Pas un Utilisateur");
            user.UtilisateurId = ((Utilisateur)result.Value).UtilisateurId;
            Assert.AreEqual(user, (Utilisateur)result.Value, "Utilisateurs pas identiques");
        }

        [TestMethod]
        public void PutUtilisateur_ValidUpdate_AvecMoq()
        {
            // Arrange
            Utilisateur userOriginal = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };

            Utilisateur userModified = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "NewName",
                Prenom = "NewFirstName",
                Mobile = "1234567890",
                Mail = "newemail@gmail.com",
                Pwd = "NewPassword123!",
                Rue = "NewAddress",
                CodePostal = "12345",
                Ville = "NewCity",
                Pays = "NewCountry",
                Latitude = 47.123456F,
                Longitude = 8.987654F
            };

            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(userOriginal);
            var userController = new UtilisateursController(mockRepository.Object);

            // Act
            var actionResult = userController.PutUtilisateur(1, userModified).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");

            // Vérifiez que les données ont été mises à jour correctement dans le mockRepository
            mockRepository.Verify(x => x.UpdateAsync(userOriginal, userModified), Times.Once);
        }


        [TestMethod()]
        public void DeleteUtilisateurTest()
        {
            Assert.Fail();
        }


        [TestMethod]
        public void DeleteUtilisateurTest_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.DeleteUtilisateur(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}