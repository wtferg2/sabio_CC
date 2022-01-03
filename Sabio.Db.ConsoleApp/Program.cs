using Sabio.Services;
using System;
using System.Data.SqlClient;
using Sabio.Models.Domain;
using System.Collections.Generic;
using Sabio.Data;
using Sabio.Models.Requests.Addresses;
using Sabio.Models;

namespace Sabio.Db.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Here are two example connection strings. Please check with the wiki and video courses to help you pick an option

            //string connString = @"Data Source=ServerName_Or_IpAddress;Initial Catalog=DB_Name;User ID=SabioUser;Password=Sabiopass1!";
            string connString = @"Data Source=104.42.194.102;Initial Catalog=C111_william.t.ferguson.jr_gmail;User ID=C111_william.t.ferguson.jr_gmail_User;Password=C111_william.t.ferguson.jr_gmail_UserAF92F063";

            TestConnection(connString);
            TestFriendsService(connString);
            TestAddressService(connString);
            TestUsersService(connString);



        }
        private static void TestUsersService(string connString)
        {
            #region - Constructor Calls - OK
            SqlDataProvider provider = new SqlDataProvider(connString);

            UsersService userService = new UsersService(provider);
            #endregion


            #region --- Gets/Selects ----
            User user = userService.Get(1);

            List<User> users = userService.GetAll();
            #endregion



            #region ------Updates/Requests-------
            UsersAddRequest request = new UsersAddRequest();

            request.UserId = "445 Sherbert";
            request.FirstName = "CA";
            request.LastName = "90802";
            request.Email = "La";
            request.Password = "Active";
            request.AvatarUrl = "url";
            request.TenantId = "12345";


            int newId = userService.Add(request);

            UsersUpdateRequest updateRequest = new UsersUpdateRequest();

            updateRequest.UserId = "445 Sherbert";
            updateRequest.FirstName = "CA";
            updateRequest.LastName = "90802";
            updateRequest.Email = "La";
            updateRequest.Password = "Active";
            updateRequest.AvatarUrl = "url";
            updateRequest.TenantId = "12345";
            updateRequest.Id = newId;

            userService.Update(updateRequest);
            User greatUser = userService.Get(newId);
            userService.Delete(4);

            #endregion


            Console.ReadLine();//This waits for you to hit the enter key before closing window
        }

        private static void TestFriendsService(string connString)
        {
            #region - Constructor Calls - OK
            SqlDataProvider provider = new SqlDataProvider(connString);

            FriendsService friendService = new FriendsService(provider);
            #endregion


            #region --- Gets/Selects ----
            Friend friend = friendService.Get(1);

            List<Friend> friends = friendService.GetAll();
            #endregion



            #region ------Updates/Requests-------
            FriendsAddRequest request = new FriendsAddRequest();

            request.Title = "445 Sherbert";
            request.Summary = "CA";
            request.Headline = "90802";
            request.Slug = "La";
            request.StatusId = "Active";
            request.PrimaryImage = "url";
            request.UserId = "12345";


            int newId = friendService.Add(request);

            FriendsUpdateRequest updateRequest = new FriendsUpdateRequest();

            updateRequest.Title = "445 Sherbert";
            updateRequest.Summary = "CA";
            updateRequest.Headline = "90802";
            updateRequest.Slug = "La";
            updateRequest.StatusId = "Active";
            updateRequest.PrimaryImage = "url";
            updateRequest.UserId = "12345";
            updateRequest.Id = newId;

            friendService.Update(updateRequest);
            Friend greatFriend = friendService.Get(newId);
            friendService.Delete(4);

            #endregion


            Console.ReadLine();//This waits for you to hit the enter key before closing window
        }
        private static void TestAddressService(string connString)
        {
            #region - Constructor Calls - OK
            SqlDataProvider provider = new SqlDataProvider(connString);

            AddressesService aService = new AddressesService(provider);


            IAddressesService addressInterface = new AddressesService(provider);
            #endregion


            #region --- Gets/Selects ----
            Address anAddress = aService.Get(7);

            List<Address> addresses = aService.GetTop();
            #endregion



            #region ------Updates/Requests-------
            AddressAddRequest request = new AddressAddRequest();

            request.LineOne = "445 Sherbert";
            request.State = "CA";
            request.PostalCode = "90802";
            request.City = "La";
            request.IsActive = true;


            int newId = aService.Add(request);

            AddressUpdateRequest updateRequest = new AddressUpdateRequest();

            updateRequest.LineOne = "4445 Sherbert";
            updateRequest.State = "GA";
            updateRequest.PostalCode = "90801";
            updateRequest.City = "ATL";
            updateRequest.IsActive = true;
            updateRequest.Id = newId;

            aService.Update(updateRequest);
            Address greatAddress = aService.Get(newId); 
            #endregion


            Console.ReadLine();//This waits for you to hit the enter key before closing window
        }
        private static void TestConnection(string connString)
        {
            bool isConnected = IsServerConnected(connString);
            Console.WriteLine("DB isConnected = {0}", isConnected);
        }

        private static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
    }
}
