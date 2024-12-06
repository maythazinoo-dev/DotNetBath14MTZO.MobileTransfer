using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using System.Transactions;

namespace DotNetBath14MTZO.MobileTransfer.Feature.MobileTransfer
{
    public class EfCoreMobileTransferService
    {
        private readonly AppDbContext _appDbContext;


        public EfCoreMobileTransferService()
        {
            _appDbContext = new AppDbContext();
        }

        public List<UserModel> GetUsers()
        {
            var users = _appDbContext.Tbl_UserModel.AsNoTracking().ToList();
            return users;
        }

        public UserModel GetMobileNo(string Mobileno)
        {
            var mno = _appDbContext.Tbl_UserModel.AsNoTracking().FirstOrDefault(x => x.MobileNo == Mobileno);

            return mno!;

        }

        public MobileResponseModel CreateUser(UserModel requestModel)
        {
            requestModel.UserId = Guid.NewGuid().ToString();
            _appDbContext.Tbl_UserModel.Add(requestModel);
            var result = _appDbContext.SaveChanges();

            string message = result > 0 ? "Saving Successful." : "Saving Failed";
            MobileResponseModel response = new MobileResponseModel();
            response.IsSuccess = result > 0;
            response.Message = message;

            return response;

        }
        public MobileResponseModel UpsertWithdraw(string mobileno, decimal balance, string password)
        {
            var usercheck = _appDbContext.Tbl_UserModel.AsNoTracking().FirstOrDefault(x => x.MobileNo == mobileno);

            if (usercheck is null)
            {
                MobileResponseModel userResponseModel = new MobileResponseModel();
                userResponseModel.IsSuccess = false;
                userResponseModel.Message = "Not found Mobile Number";
                return userResponseModel;

            }


            if (usercheck.UserPassword != password)
            {
                MobileResponseModel userResponseModel = new MobileResponseModel();
                userResponseModel.IsSuccess = false;
                userResponseModel.Message = "Not found password";
                return userResponseModel;
            }


            if (usercheck.UserBalance < balance)
            {
                MobileResponseModel userResponseModel = new MobileResponseModel();
                userResponseModel.IsSuccess = false;
                userResponseModel.Message = "Balance not enough";
                return userResponseModel;
            }

            if (balance < 10000)
            {
                MobileResponseModel userResponseModel = new MobileResponseModel();
                userResponseModel.IsSuccess = false;
                userResponseModel.Message = "Minimum withdrawal amount is 10,000.";
                return userResponseModel;

            }

            usercheck.UserBalance -= balance;
            _appDbContext.Entry(usercheck).State = EntityState.Modified;
            var result = _appDbContext.SaveChanges();
            string message = result > 0 ? "WithDraw Successful" : "WithDraw Failed";
            MobileResponseModel mobileResponseModel = new MobileResponseModel();
            mobileResponseModel.IsSuccess = true;
            mobileResponseModel.Message = "withdraw service successful";
            return mobileResponseModel;
        }

        public MobileResponseModel UpsertDeposit(string mobileno, decimal balance, string password)
        {
            var usercheck = _appDbContext.Tbl_UserModel.AsNoTracking().FirstOrDefault(x => x.MobileNo == mobileno);

            if (usercheck is null)
            {
                MobileResponseModel userResponseModel = new MobileResponseModel();
                userResponseModel.IsSuccess = false;
                userResponseModel.Message = "Not found Mobile Number";
                return userResponseModel;

            }

            if (usercheck.UserPassword != password)
            {
                MobileResponseModel userResponseModel = new MobileResponseModel();
                userResponseModel.IsSuccess = false;
                userResponseModel.Message = "Not found password";
                return userResponseModel;
            }

            if (balance < 10000)
            {
                MobileResponseModel userResponseModel = new MobileResponseModel();
                userResponseModel.IsSuccess = false;
                userResponseModel.Message = "Minimum deposit amount is 10,000.";
                return userResponseModel;

            }

            usercheck.UserBalance += balance;
            _appDbContext.Entry(usercheck).State = EntityState.Modified;
            var result = _appDbContext.SaveChanges();
            string message = result > 0 ? "Deposit Successful" : "Deposit Failed";
            MobileResponseModel mobileResponseModel = new MobileResponseModel();
            mobileResponseModel.IsSuccess = true;
            mobileResponseModel.Message = message;
            return mobileResponseModel;
        }

        public MobileResponseModel Transfer(string frommobileno, string tomobileno, decimal amount,DateTime transactiondate, string notes, string password)
        {
            var fromuser = (UserModel)GetMobileNo(frommobileno);
            var touser = (UserModel)GetMobileNo(tomobileno);

            if (fromuser == null )

            {
                MobileResponseModel userResponseModel= new MobileResponseModel();
                userResponseModel.IsSuccess = false;
                userResponseModel.Message = "FromUser Not Found";
                return userResponseModel;
            }

            if (fromuser.UserPassword != password)
            {
                MobileResponseModel userResponseModel = new MobileResponseModel();
                userResponseModel.IsSuccess = false;
                userResponseModel.Message = "Password is invalid";
                return userResponseModel;

            }

            if (touser == null)

            {
                MobileResponseModel userResponseModel = new MobileResponseModel();
                userResponseModel.IsSuccess = false;
                userResponseModel.Message = "FromUser Not Found";
                return userResponseModel;
            }

            if (fromuser.UserBalance < amount)
            {
                MobileResponseModel userResponseModel = new MobileResponseModel();
                userResponseModel.IsSuccess = false;
                userResponseModel.Message = "Insufficient balance";
                return userResponseModel;
            }


                fromuser.UserBalance-=amount;
                touser.UserBalance += amount;


            try
            {
                _appDbContext.Tbl_UserModel.Update(fromuser);
                _appDbContext.Tbl_UserModel.Update(touser);

                var transaction = new TransactionModel
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    FromMobileNo = frommobileno,
                    ToMobileNo = tomobileno,
                    Amount = amount,
                    TransactionDate = transactiondate,
                    Notes = notes

                };

                _appDbContext.Tbl_TransactionModel.Add(transaction);

               _appDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                MobileResponseModel responseModel = new MobileResponseModel();
                responseModel.IsSuccess = false;
                responseModel.Message = $"Transaction failed: {ex.Message}";
                return responseModel;
            }

            MobileResponseModel userResponse = new MobileResponseModel();
            userResponse.IsSuccess = true;
            userResponse.Message = "Transaction Success";
            return userResponse;

        }

        public List<TransactionModel> TransactionHistory(string mobileno)
        {
            try
            {
                var transactionHistory = _appDbContext.Tbl_TransactionModel
                    .Where(t => t.FromMobileNo == mobileno || t.ToMobileNo == mobileno)
                    .OrderByDescending(t => t.TransactionDate)
                    .ToList();

                return transactionHistory;
            }
            catch (Exception ex)
            {
                return new List<TransactionModel> { new TransactionModel { Notes = $"Error: {ex.Message}" } };
            }
          
        }

    
        }
    }

    
    

