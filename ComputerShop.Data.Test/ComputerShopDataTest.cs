using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using ComputerShop.Data.Context;
using ComputerShop.Data.Model;
using EntityFramework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComputerShop.Data.Test
{
    [TestClass]
    public class ComputerShopDataTest
    {
        class ComputerShopTestDropinicialier : DropCreateDatabaseAlways<Context.ComputerShop>
        {
            protected override void Seed(Context.ComputerShop context)
            {
                base.Seed(context);

                var computerShopTestInitializer = new ComputerShopTestInitializerOperations();
                computerShopTestInitializer.Seed(context);
            }
        }

        class ComputerShopTestInitializerOperations : ComputerShopInitializerOperations
        {
            protected override void SeedEntities(Context.ComputerShop context)
            {
                base.SeedEntities(context);

                var computerBrands = new List<ComputerBrand>
                    {
                        new ComputerBrand()
                            {
                                Name = "Dell"
                            },
                        new ComputerBrand()
                            {
                                Name = "HP"
                            },
                    };

                computerBrands.ForEach(computerBrand => context.CompBrands.Add(computerBrand));
                context.SaveChanges();

                int counter = 0;
                foreach(var computerBrand in context.CompBrands)
                {
                    context.Computers.Add(new Computer()
                        {
                            ComputerBrand = computerBrand,
                            Description = "some PC " + counter
                        });

                    counter++;
                }
                context.SaveChanges();
            }
        }

        [TestInitialize]
        public void SetUp()
        {
            Database.SetInitializer(new ComputerShopTestDropinicialier());
        }

        [TestMethod]
        public void InitDB()
        {
            var testee = new Context.ComputerShop();

            var computers = from c in testee.Computers
                            select c;

            DbContextMetadata.FindEntities(testee);

            Assert.IsTrue(computers.Any());

            var computerBrands = from c in testee.CompBrands
                                 select c;

            Assert.IsTrue(computerBrands.Any());
        }

        [TestMethod]
        public void TestStps()
        {
            var testee = new Context.ComputerShop();
            
            Assert.IsNotNull(testee.ComputerStps);

            var stps = testee.ComputerStps;

            var computers = from c in testee.Computers
                            select c;

            var computer = computers.First();
            computer.Description = "new description";

            stps.GetUpdateStp(computer).Execute(testee);

            computers = from c in testee.Computers
                             where c.Id == computer.Id
                            select c;
            
            Assert.IsTrue(computers.SingleOrDefault() != null);

            stps.GetDeleteStp(computer.Id).Execute(testee);

            computers = from c in testee.Computers
                        where c.Id == computer.Id
                        select c;

            Assert.IsTrue(!computers.Any());

            var newComputer = new Computer()
                {
                    Description = computer.Description,
                    ComputerBrand = computer.ComputerBrand,
                    ComputerModel = computer.ComputerModel,
                    HarddiskCapacity = computer.HarddiskCapacity,
                    HarddiskCapacityUnit = computer.HarddiskCapacityUnit,
                    Processor = computer.Processor,
                    RamCapacity = computer.RamCapacity,
                    RamUnit = computer.RamUnit,

                };

            stps.GetInsertStp(newComputer).Execute(testee);

            computers = from c in testee.Computers
                        where c.Description == computer.Description
                        select c;

            Assert.IsTrue(computers.SingleOrDefault() != null);
        }

        [TestMethod]
        public void FindTableMapping()
        {
            var testee = new Context.ComputerShop();

            var computers = from c in testee.Computers
                            select c;

            var some = DbContextMetadata.FindEntityToTableMapping<Computer>(testee).ToList();

            var keyMetadata = some.First();

            Assert.AreEqual("Id", keyMetadata.Key);
            Assert.AreEqual("CompID", keyMetadata.Value.Name);
            Assert.AreEqual("int", keyMetadata.Value.TypeUsage.EdmType.Name);
        }
    }
}
