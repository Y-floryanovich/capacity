﻿using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teams.Data;
using Teams.Models;
using Teams.Repository;

namespace Teams.Tests
{
    class TeamMembersRepositoryTest
    {
        [TestFixture]
        class TeamRepositoryTest
        {
            private ApplicationDbContext context;
            private IRepository<TeamMember, int> teamMemberRepository;

            [SetUp]
            public void Setup()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TeamMemberListDatabase")
                .Options;

                context = new ApplicationDbContext(options);

            }

            private IQueryable<TeamMember> GenerateData(int num)
            {
                var lst = new List<TeamMember>();
                for (int i = 1; i < num; i++)
                {
                    lst.Add(new TeamMember
                    {
                        Id = i,
                        MemberId = i.ToString(),
                    });
                }
                return lst.AsQueryable();
            }

            #region GetAll
            [Test]
            public void GetAll_TeamRepositoryReturnsListCount100_ListCount100()
            {
                //Arrange
                context.TeamMembers.AddRange(GenerateData(101));
                context.SaveChanges();
                teamMemberRepository = new TeamMemberRepository(context);

                //Act
                var teamMembers = teamMemberRepository.GetAll().Result;

                //Assert
                Assert.AreEqual(context.TeamMembers.Count(), teamMembers.Count());

            }

            [Test]
            public void GetAll_TeamRepositoryReturnsEmptyList_ReturnsEmpty()
            {
                //Arrange
                context.TeamMembers.AddRange(GenerateData(0));
                context.SaveChanges();
                teamMemberRepository = new TeamMemberRepository(context);

                //Act
                var teams = teamMemberRepository.GetAll().Result;

                //Assert
                Assert.IsEmpty(teams);

            }
            #endregion

            #region GetByIdAsync
            [Test]
            public void GetByIdAsync_TeamRepositoryReturnsTeamId1_ReturnsTeamId1()
            {
                //Arrange
                context.TeamMembers.AddRange(GenerateData(100));
                context.SaveChanges();
                teamMemberRepository = new TeamMemberRepository(context);

                //Act
                var team = teamMemberRepository.GetByIdAsync(1).Result;

                //Assert
                Assert.AreEqual(1, team.Id);

            }

            [Test]
            public void GetByIdAsync_TeamRepositoryReturnsNull_ReturnsNull()
            {
                //Arrange
                context.TeamMembers.AddRange(GenerateData(100));
                context.SaveChanges();
                teamMemberRepository = new TeamMemberRepository(context);

                //Act
                var team = teamMemberRepository.GetByIdAsync(101).Result;

                //Assert
                Assert.IsNull(team);

            }
            #endregion

            #region InsertAsync
            [Test]
            public void InsertAsync_TeamRepositoryReturnsTrue_ReturnsTrue()
            {
                //Arrange
                context.TeamMembers.AddRange(GenerateData(100));
                context.SaveChanges();
                teamMemberRepository = new TeamMemberRepository(context);
                TeamMember teamMember = new TeamMember() { MemberId = "100" };

                //Act
                var result = teamMemberRepository.InsertAsync(teamMember).Result;

                //Assert
                Assert.IsTrue(result);

            }
            #endregion

            #region DeleteAsync
            [Test]
            public void DeleteAsync_TeamRepositoryReturnsTrue_ReturnsTrue()
            {
                //Arrange
                context.TeamMembers.AddRange(GenerateData(100));
                context.SaveChanges();
                teamMemberRepository = new TeamMemberRepository(context);

                //Act
                var result = teamMemberRepository.DeleteAsync(teamMemberRepository.GetByIdAsync(1).Result).Result;

                //Assert
                Assert.IsTrue(result);
            }

            [Test]
            public void DeleteAsync_TeamRepositoryReturnsNull_ReturnsNull()
            {
                //Arrange
                context.TeamMembers.AddRange(GenerateData(100));
                context.SaveChanges();
                teamMemberRepository = new TeamMemberRepository(context);

                //Act&Assert
                try
                {
                    var result = teamMemberRepository.DeleteAsync(null);
                }
                catch (Exception e)
                {
                    Assert.AreEqual("Value cannot be null.", e.Message, string.Format("result != expected"));
                }

            }
            #endregion

            #region UpdateAsync
            [Test]
            public void UpdateAsync_TeamRepositoryReturnsTrue_ReturnsTrue()
            {
                //Arrange
                context.TeamMembers.AddRange(GenerateData(100));
                context.SaveChanges();
                teamMemberRepository = new TeamMemberRepository(context);

                //Act
                var result = teamMemberRepository.UpdateAsync(new TeamMember() { Id = 1, MemberId = "01" }).Result;

                //Assert
                Assert.IsTrue(result);

            }

            [Test]
            public void UpdateAsync_TeamRepositoryReturnsFalse_ReturnsFalse()
            {
                //Arrange
                context.TeamMembers.AddRange(GenerateData(100));
                context.SaveChanges();
                teamMemberRepository = new TeamMemberRepository(context);

                //Act
                var result = teamMemberRepository.UpdateAsync(new TeamMember() { Id = 100, MemberId = "001" }).Result;

                //Assert
                Assert.IsFalse(result);

            }
            #endregion
        }

    }
}
