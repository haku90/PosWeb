using System;
using PosWeb.Models;
using PosWeb.Repositories;
using PosWeb.Repositories.Infrastructure;

namespace PosWeb.Services
{
    public class TestService
    {
        private TestRepository _testRepository;
        private IUnitOfWork _unitOfWork;

        public TestService(TestRepository testRepository, IUnitOfWork unitOfWork)
        {
            _testRepository = testRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddTestObject()
        {
           
            _testRepository.Add(new TestModel{
                
            });
            _unitOfWork.Commit();
        }
    }
}
