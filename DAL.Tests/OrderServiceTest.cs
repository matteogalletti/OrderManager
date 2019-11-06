using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Contracts;
using Contracts.DTO;
using DAL.Services;
using Domain.Models;
using Moq;
using NUnit.Framework;

namespace DAL.Tests
{
    [TestFixture]
    public class OrderServiceTest
    {
        [Test]
        public void CreateOrder_returns_failure_if_id_is_specified()
        {
            var dto = new OrderDTO
            {
                Id = "abcde"
            };

            var svc = new OrderService(null);

            var orderId = svc.CreateOrder(dto, DateTime.Now, out string errorMessage);

            Assert.IsNull(orderId);
            Assert.AreEqual("Id must be null", errorMessage);
        }

        [Test]
        public void CreateOrder_returns_failure_if_order_contains_no_products()
        {
            var dto = new OrderDTO
            {
                Products = new List<ProductDTO>()
            };

            var svc = new OrderService(null);

            var orderId = svc.CreateOrder(dto, DateTime.Now, out string errorMessage);

            Assert.IsNull(orderId);
            Assert.AreEqual("Empty product list", errorMessage);
        }

        [Test]
        public void CreateOrder_returns_failure_if_order_contains_products_with_invalid_ids()
        {
            var dto = new OrderDTO
            {
                Products = new List<ProductDTO>
                {
                    new ProductDTO
                    {
                        Name = "Product with valid id",
                        Id = "6aff86cf-caaf-4440-acad-2ee8bc4abfb0"
                    },
                    new ProductDTO
                    {
                        Name = "Product with invalid id",
                        Id = "abc"
                    }
                }
            };

            var svc = new OrderService(null);
            var orderId = svc.CreateOrder(dto, DateTime.Now, out string errorMessage);

            Assert.IsNull(orderId);
            Assert.AreEqual("Order contains products with invalid id", errorMessage);
        }

        [Test]
        public void CreateOrder_returns_failure_if_order_contains_not_existing_products()
        {
            var dto = new OrderDTO
            {
                Products = new List<ProductDTO>
                {
                    new ProductDTO
                    {
                        Name = "Product with valid id",
                        Id = "6aff86cf-caaf-4440-acad-2ee8bc4abfb0"
                    },
                    new ProductDTO
                    {
                        Name = "Not existing product",
                        Id = "f1f62327-e56b-455d-8b6e-9b987e844d2a"
                    }
                }
            };

            var productSearchResult = new List<Product>
            {
                new Product
                {
                    Id = Guid.Parse("6aff86cf-caaf-4440-acad-2ee8bc4abfb0")
                }
            }.AsQueryable();
            var productRepoMock = new Mock<IBaseRepository<Product>>();
            productRepoMock.Setup(r => r.Find(It.IsAny<Expression<Func<Product, bool>>>()))
                .Returns(productSearchResult);
            
            
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.SetupProperty(uow => uow.ProductRepository, productRepoMock.Object);

            var svc = new OrderService(uowMock.Object);

            var orderId = svc.CreateOrder(dto, DateTime.Now, out string errorMessage);
            Assert.IsNull(orderId);
            Assert.AreEqual("Order contains not existing products", errorMessage);

        }

        [Test]
        public void CreateOrder_returns_failure_if_products_do_not_belong_to_the_same_category()
        {
            var dto = new OrderDTO
            {
                Products = new List<ProductDTO>
                {
                    new ProductDTO
                    {
                        Name = "Product A",
                        Id = "6aff86cf-caaf-4440-acad-2ee8bc4abfb0"
                    },
                    new ProductDTO
                    {
                        Name = "Product B",
                        Id = "f1f62327-e56b-455d-8b6e-9b987e844d2a"
                    }
                }
            };

            var productSearchResult = new List<Product>
            {
                new Product
                {
                    Id = Guid.Parse("6aff86cf-caaf-4440-acad-2ee8bc4abfb0"),
                    Category = new Category
                    {
                        Id = Guid.NewGuid()
                    }
                },
                new Product
                {
                    Id = Guid.Parse("f1f62327-e56b-455d-8b6e-9b987e844d2a"),
                    Category = new Category
                    {
                        Id = Guid.NewGuid()
                    }
                }
            }.AsQueryable();
            var productRepoMock = new Mock<IBaseRepository<Product>>();
            productRepoMock.Setup(r => r.Find(It.IsAny<Expression<Func<Product, bool>>>()))
                .Returns(productSearchResult);


            var uowMock = new Mock<IUnitOfWork>();
            uowMock.SetupProperty(uow => uow.ProductRepository, productRepoMock.Object);

            var svc = new OrderService(uowMock.Object);

            var orderId = svc.CreateOrder(dto, DateTime.Now, out string errorMessage);
            Assert.IsNull(orderId);
            Assert.AreEqual("Products do not belong to same category", errorMessage);
        }
    }
}