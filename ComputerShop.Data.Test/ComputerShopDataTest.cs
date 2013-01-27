using System;
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
        class ComputerShopTestDropinicialier : DropCreateDatabaseAlways<Context.ComputerShopContext>
        {
            protected override void Seed(ComputerShopContext context)
            {
                base.Seed(context);

                var computerShopTestInitializer = new ComputerShopTestInitializerOperations();
                computerShopTestInitializer.Seed(context);
            }
        }

        class ComputerShopTestInitializerOperations : ComputerShopInitializerOperations
        {
            protected override void SeedEntities(Context.ComputerShopContext context)
            {
                base.SeedEntities(context);

                var computerBrands = new List<ComputerBrand>
                    {
                        new ComputerBrand()
                            {
                                Name = "Dell",
                                Description = "just dell"
                            },
                        new ComputerBrand()
                            {
                                Name = "HP",
                                Description = "just hp"
                            },
                        new ComputerBrand()
                            {
                                Name = "Lenovo",
                                Description = "just Lenovo"
                            }
                    
                    };

                computerBrands.ForEach(computerBrand => context.CompBrands.Add(computerBrand));
                context.SaveChanges();

                var processors = new List<Processor>
                                     {
                                         new Processor
                                             {
                                                 Name = "Amd",
                                                 Description = "just amd"
                                             },
                                         new Processor
                                             {
                                                 Name = "Intel",
                                                 Description = "just intel"
                                             },
                                     };
                processors.ForEach(processor => context.Processors.Add(processor));
                context.SaveChanges();

                var prices = new List<decimal>
                                 {
                                     500, 699, 399m, 1299.99m
                                 };

                var computerModels = new List<string>
                {
                    "Elite 7500", "Pavilon P6-2303ec", "IdeaCentre", "Vostro"
                };

                var ramCapacity = new List<int>
                                   {
                                       2,
                                       4,
                                       3,
                                       8,
                                       16
                                   };

                var hddCapacity = new List<int>
                                      {
                                          500, 750, 1000, 2000, 3000
                                      };

                int counter = 0;

                
                foreach(var computerBrand in context.CompBrands)
                {
                    for (var i = 0; i < 10; i ++ )
                    {
                        context.Computers.Add(new Computer()
                            {
                                ComputerBrand = computerBrand,
                                Description = "some PC " + i,
                                Processor = processors[i%2],
                                Price = prices[i%4],
                                ComputerModel = computerModels[i%4],
                                RamCapacity = ramCapacity[i%5],
                                RamUnit = CapacityUnitEnum.GB,
                                HarddiskCapacity = hddCapacity[i%5],
                                HarddiskCapacityUnit = CapacityUnitEnum.GB,
                                
                            });
                    }

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
            var testee = new Context.ComputerShopContext();

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
            var testee = new Context.ComputerShopContext();
            
            Assert.IsNotNull(testee.ComputerStps);

            var stps = testee.ComputerStps;

            var computers = from c in testee.Computers
                            select c;

            var computer = computers.First();
            computer.Description = new Random(2244).Next() + "new description" + new Random(2255).Next();

            stps.GetUpdateStp(computer).Execute(testee);

            computers = from c in testee.Computers
                             where c.Id == computer.Id
                            select c;
            
            Assert.IsTrue(computers.SingleOrDefault() != null);
            Assert.IsTrue(computers.Single().Description == computer.Description);

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
                    Price = computer.Price
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
            var testee = new Context.ComputerShopContext();

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
