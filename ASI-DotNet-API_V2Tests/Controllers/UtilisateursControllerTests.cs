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

namespace ASI_DotNet_API_V2.Controllers.Tests
{
    [TestClass()]
    public class UtilisateursControllerTests
    {
        private ASIDBContext _context;
        private UtilisateursController _controller;

        public UtilisateursControllerTests()
        {
            var builder = new DbContextOptionsBuilder<ASIDBContext>().UseNpgsql("Server = localhost; port = 5432; Database = ASIDB; uid = postgres; password = postgres; ");
            _context = new ASIDBContext(builder.Options);
            _controller = new UtilisateursController(_context);
        }

        [TestMethod]
        public void GetAll()
        {
            var users = _context.Utilisateurs.ToList();

            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]
        public void GetById()
        {
            int userIdToRetrieve = 1;

            var user = _context.Utilisateurs.Where(c => c.UtilisateurId == userIdToRetrieve).FirstOrDefault();

            Assert.IsNotNull(user);
            Assert.AreEqual(userIdToRetrieve, user.UtilisateurId);
        }

        [TestMethod]
        public async Task PostUtilisateur_ModelValidated_CreationOK()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);

            // Le mail doit être unique en concaténant un numéro aléatoire
            string uniqueEmail = "testuser" + chiffre + "@example.com";

            Utilisateur userAtester = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = uniqueEmail,
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };

            // Act
            var result = await _controller.PostUtilisateur(userAtester);

            // Assert
            Assert.IsNotNull(result);

            // Vérification que l'utilisateur a été créé dans la base de données
            Utilisateur userRecupere = _context.Utilisateurs.Where(u => u.Mail.ToUpper() == userAtester.Mail.ToUpper()).FirstOrDefault();
            Assert.IsNotNull(userRecupere);

            // Comparaison des propriétés de l'utilisateur créé avec l'utilisateur d'origine
            Assert.AreEqual(userAtester.Nom, userRecupere.Nom);
            Assert.AreEqual(userAtester.Prenom, userRecupere.Prenom);
            Assert.AreEqual(userAtester.Mobile, userRecupere.Mobile);
            Assert.AreEqual(userAtester.Mail, userRecupere.Mail);
            Assert.AreEqual(userAtester.Rue, userRecupere.Rue);
            Assert.AreEqual(userAtester.CodePostal, userRecupere.CodePostal);
            Assert.AreEqual(userAtester.Ville, userRecupere.Ville);
            Assert.AreEqual(userAtester.Pays, userRecupere.Pays);
            Assert.AreEqual(userAtester.Latitude, userRecupere.Latitude);
            Assert.AreEqual(userAtester.Longitude, userRecupere.Longitude);
        }


        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public async Task PostUtilisateur_InvalidData_ThrowsException()
        {
            Utilisateur userWithInvalidData = new Utilisateur()
            {
                Nom = null,
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "testuser@example.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };

            _controller.PostUtilisateur(userWithInvalidData).Wait();
        }

        [TestMethod()]
        public void DeleteUtilisateurTest()
        {
            Assert.Fail();
        }
    }
}