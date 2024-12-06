using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBath14MTZO.MobileTransfer.Feature.MobileTransfer
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileTransferController : ControllerBase
    {
        private readonly EfCoreMobileTransferService _efCoreMobileTransferService;

        public MobileTransferController()
        {
            _efCoreMobileTransferService = new EfCoreMobileTransferService();
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            var MobileTransferModel = _efCoreMobileTransferService.GetUsers();
            return Ok(MobileTransferModel);
        }

     
        [HttpGet("Mobileno")]
        public IActionResult GetMobileNo(string Mobileno)
        {
            var result =_efCoreMobileTransferService.GetMobileNo(Mobileno);
            if (result is null)
            {
                return NotFound("Not found Mobile Number");
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateUser(UserModel requestModel)
        {
            var usermodel =_efCoreMobileTransferService.CreateUser(requestModel);
            if (!usermodel.IsSuccess) 
            {
                return BadRequest(usermodel);
            }
            return Ok(usermodel);

        }
        [HttpPatch("Withdraw")]
        public IActionResult UpsertWithdraw(string mobileno, decimal balance, string password)
        {
            var usermodel = _efCoreMobileTransferService.UpsertWithdraw(mobileno, balance, password);
            if (!usermodel.IsSuccess)
            {
                return BadRequest(usermodel);

            }
            return Ok(usermodel);

        }

        [HttpPatch("Deposit")]
        public IActionResult UpsertDeposit(string mobileno, decimal balance, string password)
        {
            var usermodel = _efCoreMobileTransferService.UpsertDeposit(mobileno, balance, password);
            if (!usermodel.IsSuccess)
            {
                return BadRequest(usermodel);

            }
            return Ok(usermodel);

        }

        [HttpPatch("Transaction")]
        public IActionResult Transfer(string frommobileno,string tomobileno,decimal balance,DateTime transactiondate,string notes,string password)
        {
            var usermodel = _efCoreMobileTransferService.Transfer(frommobileno,tomobileno,balance,transactiondate,notes,password);
            if (!usermodel.IsSuccess)
            {
                return BadRequest(usermodel);
            }
            return Ok(usermodel);


        }

        [HttpGet("TransactionHistory")]
        public IActionResult GetHistory(string mobileno)
        {
            
            var histroy =_efCoreMobileTransferService.TransactionHistory(mobileno);
            return Ok(histroy);
        }
    }
}

