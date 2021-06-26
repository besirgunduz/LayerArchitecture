

using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService; //dikkat et service'i alıyorsun.
        }

        [SecuredOperation("Product.Add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]//Get ile başlayan metottaki bütün cacheleri siler
        public IResult Add(Product product)
        {
            // kuralların hepsine uyuyorsa null,uymuyorsa uymayanı getirir.
            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName), CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfCategoryCount());

            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;

            if (result >= 15)
            {
                return new ErrorResult("Ürün sayısı 15 ten küçük olmalı");
            }

            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();

            if (result)
            {
                return new ErrorResult("Ürün ismi zaten var");
            }

            return new SuccessResult();
        }

        private IResult CheckIfCategoryCount()
        {
            var result = _categoryService.GetAll();

            if (result.Data.Count > 15)
            {
                return new ErrorResult("Kategori sayısı 15'yi geçemez");
            }

            return new SuccessResult();
        }

        [SecuredOperation("Product.List,Admin")]
        [CacheAspect]
        [PerformanceAspect(5)] //eğer 5 sn yi geçerse bize bildirir.
        [LogAspect(typeof(DatabaseLogger))]
        public IDataResult<List<Product>> GetAll()
        {
            Thread.Sleep(5000); //performans takip testi

            if (DateTime.Now.Hour == 03)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        [SecuredOperation("Product.List,Admin")]
        [CacheAspect(20)]
        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        [CacheAspect(1)]
        [LogAspect(typeof(DatabaseLogger))]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            if (DateTime.Now.Hour == 01)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            var result = _productDal.GetAll(p => p.CategoryId == product.CategoryId).Count;
            if (result > 10)
            {
                return new ErrorResult("Kategorideki ürün sayısı en fazla 10 olabilir.");
            }

            return new SuccessResult();


        }

        [TransactionScopeAspect]
        public IResult TransactionalTest(Product product)
        {
            //_productDal.Update(product);
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }
    }
}
