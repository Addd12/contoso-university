﻿using ContosoUniversity.Controllers;
using ContosoUniversity.Data.Entities;
using ContosoUniversity.Data.Interfaces;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ContosoUniversity.UnitTests.Web
{
    public class HomeControllerTests
    {
        private readonly ITestOutputHelper _output;
        private readonly Mock<IPersonRepository<Student>> mockStudentRepo;
        HomeController _sut;
        
        public HomeControllerTests(ITestOutputHelper output)
        {
            _output = output;
            mockStudentRepo = Students().AsMockPersonRepository();
            _sut = new HomeController(mockStudentRepo.Object);
        }

        [Fact]
        public void Index_ReturnsAViewResult()
        {
            var result = _sut.Index();

            Assert.IsType(typeof(ViewResult), result);
        }

        //todo: test about action

        [Fact]
        public void Contact_ReturnsAViewResult()
        {
            var result = _sut.Contact();

            Assert.IsType(typeof(ViewResult), result);
        }

        [Fact]
        public void Error_ReturnsAViewResult()
        {
            var result = _sut.Error();

            Assert.IsType(typeof(ViewResult), result);
        }

        private List<Student> Students()
        {
            return new List<Student>
                {
                    new Student{ ID = 1, FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
                    new Student{ ID = 2, FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{ ID = 3, FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
                    new Student{ ID = 4, FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{ ID = 5, FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{ ID = 6, FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
                    new Student{ ID = 7, FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
                    new Student{ ID = 8, FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}
            };
        }
    }
}