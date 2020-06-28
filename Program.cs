
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using System.Diagnostics;

namespace TwitterBotDotNet
{
    class Program
    {
        //TODO: fix reference for talkToTInvi
        //TODO: close api_keys file??



        // directs user browser to twitter.com
        public static void OpenChrome()
        {
            // url's are not considered documents. They can only be opened
            // by passing them as arguments.
            Process.Start("Chrome.exe", "http://www.twitter.com/@autoBot04768645");
        }


        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Bot started");

            /***login***/

            //read passwords from file here using your own file path

            //string pathOfApiKeys = @"C:\Users\apaig\Documents\VSRepo\TwitterBotRemastered\api_keys.txt";
            string pathOfApiKeys = @".\api_keys.txt";
            //read file and put contents into array
            string[] allKeys = File.ReadAllLines(pathOfApiKeys);

            string ApiKey = allKeys[0];
            string ApiKeySecret = allKeys[1];
            string AccessToken = allKeys[2];
            string AccessTokenSecret = allKeys[3];

            // Set up your credentials (https://apps.twitter.com)
            Auth.SetUserCredentials(ApiKey, ApiKeySecret, AccessToken, AccessTokenSecret);

            //Login
            var user = User.GetAuthenticatedUser();
            if (user != null)
            {
                Console.WriteLine("Login Successful");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could Not Login, Check Credentials or Internet Connection");
                Console.ResetColor();
            }
            Console.WriteLine("\n");


            //program options
            string[] UserOptions = { "Text Only Tweet",
                                    "Picture and Text Tweet",
                                    "Schedule Tweet for later",
                                    "Schedule Media Tweet for later",
                                    "scrape and tweet"
            };

            Console.WriteLine("Choose an option by typing the number");
            for (int i = 0; i < UserOptions.Length; i++)
            {
                Console.WriteLine(i + 1 + "). " + UserOptions[i]);
            } //display menu
            Console.WriteLine("\n");

            string userInput = Console.ReadLine();
            Console.WriteLine("\n");


            if (userInput == "1")
            {
                /************************publish the Tweet "text" on your Timeline**************************/
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("What would you like to say on twitter?");
                Console.ResetColor();
                string textToTweet = Console.ReadLine();
                Tweet.PublishTweet(textToTweet);

                //check twitter
                Console.WriteLine("Would you like to check twitter to make sure? Select: Yes or No");
                string checkTwitter = Console.ReadLine();
                if (checkTwitter.ToLower() == "yes" || checkTwitter.ToLower() == "y")
                {
                    //opens chrome to twitter page
                    OpenChrome();
                }

            }   //tweet

            else if (userInput == "2")
            {
                /**************************publish media with a caption*******************************/

                /******promt user to pick a picture******/
                Console.Write("INSTRUCTIONS: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Select the picture you want to post");
                Console.WriteLine("\n");



                /******working with the picture files******/
                //must be in .jpg

                /******file name of all pictures******/
                string pathOfPics = @".\twitterImg";
                //gets each file in directory
                string[] files = Directory.GetFiles(pathOfPics);

                //find file name and add to list
                List<string> fileNames = new List<string>();
                for (int iFile = 0; iFile < files.Length; iFile++)
                {
                    //grabs each file name
                    string fn = new FileInfo(files[iFile]).Name;
                    //and adds it to the list
                    fileNames.Add(fn);
                } //grabs each file name


                //ask user if they want to see the list of available pictures
                Console.Write("Would you like to see the available picture files to choose from?");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" Select: YES or NO");
                string showAllPictureNames = Console.ReadLine();
                Console.WriteLine("\n");

                if (showAllPictureNames == "yes" || showAllPictureNames == "" || showAllPictureNames == "y")
                {
                    //set color of files
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;

                    //write file names to console
                    for (int i = 0; i < fileNames.Count; i++)
                    {
                        Console.WriteLine(i + 1 + ") " + fileNames[i]);
                    }
                    Console.ResetColor();
                    Console.WriteLine("\n");
                }
                else if (showAllPictureNames == "no" || showAllPictureNames == "n")
                {
                    //in case I want a message for no search
                }
                else
                {
                    Console.WriteLine("You must say YES or NO");
                }


                /******search picture files******/
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Would you like to search for a file by name?");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" Select: YES or NO");
                string userSearch = Console.ReadLine().ToLower();
                Console.WriteLine("\n");

                //user input : search YES or NO
                if (userSearch == "yes" || userSearch == "" || userSearch == "y")
                {
                    //added user input into for loop to write all file containing search word
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Search for a picture file");
                    //user input: search by keyword
                    userSearch = Console.ReadLine();
                    Console.WriteLine("\n");
                    List<string> searchedFiles = new List<string>();
                    if (userSearch != "")
                    {
                        //color
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        for (int i = 0; i < fileNames.Count; i++)
                        {
                            //searches for a keyword
                            bool searchedFileNames = fileNames[i].Contains(userSearch);
                            if (searchedFileNames == true)
                            {
                                //list to hold results
                                searchedFiles.Add(fileNames[i]);
                                //prints out files containing that keyword
                                Console.WriteLine(i + 1 + ") " + fileNames[i]);
                            }
                            Console.ResetColor();
                        }


                        //when no results are found, print a message
                        if (searchedFiles.Count() == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("no results were found");
                            Console.ResetColor();
                            Console.WriteLine("\n");
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n");
                        Console.WriteLine("End of results");
                        Console.ResetColor();
                        Console.WriteLine("\n");
                    }
                }
                else if (userSearch == "no" || userSearch == "n")
                {
                    //in case I want a message for no search
                }
                else
                {
                    Console.WriteLine("You must say YES or NO, Select a picture from the numbers 1 to " + fileNames.Count + 1);
                }

                /******select picture files******/
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Select the picture by entering the number to the left of it");

                Console.ResetColor();


                //user input : picture number
                string userChoicePicture = Console.ReadLine();
                //convert string to int && -1 to grab actual index
                int realUserChoice = Convert.ToInt32(userChoicePicture) - 1;

                //full path of a file selected
                string filePath = pathOfPics + @"\" + fileNames[realUserChoice].ToString();

                //user input : picture caption
                Console.WriteLine("\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("What would you like to say with your picture on twitter?");
                Console.ResetColor();
                //capture input
                string textToTweet = Console.ReadLine();
                Console.WriteLine("\n");



                /*********send picture tweet**********/
                //exception unhandled if no input
                byte[] file1 = File.ReadAllBytes(filePath);
                var media = Upload.UploadBinary(file1);
                Tweet.PublishTweet(textToTweet + " " + DateTime.Now, new PublishTweetOptionalParameters
                {
                    Medias = new List<IMedia> { media }
                });

                //check twitter
                Console.WriteLine("Would you like to check twitter to make sure? Select: Yes or No");
                string checkTwitter = Console.ReadLine();
                if (checkTwitter.ToLower() == "yes" || checkTwitter.ToLower() == "y")
                {
                    //opens chrome to twitter page
                    OpenChrome();
                }

            }   // tweet media

            else if (userInput == "3")
            {
                /**************************schedule a tweet*******************************/

                DateTime currentTime = DateTime.Now;
                Console.WriteLine("You will post a tweet at a later date");
                Console.WriteLine("How many days do you want to wait?");
                int userAddDays = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("How many hours do you want to wait?");
                int userAddHours = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("How many minutes do you want to wait?");
                int userAddMinutes = Convert.ToInt32(Console.ReadLine());

                //get user info
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("What would you like to say on twitter?");
                Console.ResetColor();
                string textToTweet = Console.ReadLine();

                /***********************add time to the current time************************/

                //get current time
                Console.WriteLine("Current time = {0}", currentTime);

                //add days, hours, minutes
                DateTime newTime = DateTime.Now
                    .AddDays(userAddDays)
                    .AddHours(userAddHours)
                    .AddMinutes(userAddMinutes);


                /**********start helper program************/
                //pass in the path of the helper program
                string pathOfHelperProgram = @".\TwitterBotDotNetHelper.exe";

                ProcessStartInfo startInfo = new ProcessStartInfo(pathOfHelperProgram);


                //TODO: string interpolation
                startInfo.Arguments = string.Format("{0} \"{1}\" {2} {3} {4}", userInput, textToTweet, newTime, newTime, newTime);

                Process.Start(startInfo);

                //print time of scheduled post
                Console.WriteLine("Your tweet will be published at " + newTime);

            }   //tweet later

            else if (userInput == "4")
            {
                //add media to text tweet

                /**************************schedule a tweet*******************************/

                DateTime currentTime = DateTime.Now;
                Console.WriteLine("You will post a media tweet at a later date");
                Console.WriteLine("\n");
                Console.WriteLine("How many days do you want to wait?");
                int userAddDays = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("How many hours do you want to wait?");
                int userAddHours = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("How many minutes do you want to wait?");
                int userAddMinutes = Convert.ToInt32(Console.ReadLine());

                /***********get user info***************/

                //get picture

                /******promt user to pick a picture******/
                Console.Write("INSTRUCTIONS: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Select the picture you want to post");
                Console.WriteLine("\n");


                /******working with the picture files (must be in .jpg)******/

                /**file name of all pictures**/
                string pathOfPics = @".\twitterImg";
                //gets each file in directory
                string[] files = Directory.GetFiles(pathOfPics);

                //find file name and add to list
                List<string> fileNames = new List<string>();
                for (int iFile = 0; iFile < files.Length; iFile++)
                {
                    //grabs each file name
                    string fn = new FileInfo(files[iFile]).Name;
                    //and adds it to the list
                    fileNames.Add(fn);
                }


                //ask user if they want to see the list of available pictures
                Console.Write("Would you like to see the available picture files to choose from?");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" Select: YES or NO");
                string showAllPictureNames = Console.ReadLine().ToLower();
                Console.WriteLine("\n");

                //list of all pictures
                if (showAllPictureNames == "yes" || showAllPictureNames == "" || showAllPictureNames == "y")
                {
                    //set color of files
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;

                    //write file names to console
                    for (int i = 0; i < fileNames.Count; i++)
                    {
                        Console.WriteLine(i + 1 + ") " + fileNames[i]);
                    }
                    Console.ResetColor();
                    Console.WriteLine("\n");
                }
                else if (showAllPictureNames == "no" || showAllPictureNames == "n")
                {
                    //in case I want a message for no search
                }
                else
                {
                    Console.WriteLine("You must say YES or NO");
                }

                /******search picture files******/
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Would you like to search for a file by name?");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" Select: YES or NO");
                string userSearch = Console.ReadLine().ToLower();
                Console.WriteLine("\n");

                //user input : search YES or NO
                if (userSearch == "yes" || userSearch == "" || userSearch == "y")
                {
                    //added user input into for loop to write all file containing search word
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Search for a picture file");
                    Console.ResetColor();

                    //user input: search by keyword
                    userSearch = Console.ReadLine();
                    Console.WriteLine("\n");
                    List<string> searchedFiles = new List<string>();
                    if (userSearch != "")
                    {
                        //color
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;


                        for (int i = 0; i < fileNames.Count; i++)
                        {
                            //searches for a keyword
                            bool searchedFileNames = fileNames[i].Contains(userSearch);
                            if (searchedFileNames == true)
                            {
                                //list to hold results
                                searchedFiles.Add(fileNames[i]);
                                //prints out files containing that keyword
                                Console.WriteLine(i + 1 + ") " + fileNames[i]);
                            }

                        }

                        //when no results are found, print a message
                        if (searchedFiles.Count() == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("no results were found");
                            Console.ResetColor();
                            Console.WriteLine("\n");
                        }

                        //end of picture list
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n");
                        Console.WriteLine("End of results");
                        Console.ResetColor();
                        Console.WriteLine("\n");
                    }
                }
                else if (userSearch == "no" || userSearch == "n")
                {
                    //blank on purpose
                }
                else
                {
                    Console.WriteLine("You must say YES or NO");
                }


                /******select picture files******/
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Select the picture by entering the number to the left of it");
                Console.ResetColor();

                //user input : picture number
                string userChoicePicture = Console.ReadLine();
                //convert string to int && -1 to grab actual index
                int realUserChoice = Convert.ToInt32(userChoicePicture) - 1;

                //full path of a file selected
                string filePath = pathOfPics + @"\" + fileNames[realUserChoice].ToString();

                //user input : picture caption
                Console.WriteLine("\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("What would you like to say with your picture on twitter?");
                Console.ResetColor();
                //capture input
                string captionToTweet = Console.ReadLine();
                Console.WriteLine("\n");

                /*add time to the current time*/
                //get current time
                Console.WriteLine("Current time = {0}", currentTime);

                //add days, hours, minutes
                DateTime newTime = DateTime.Now
                    .AddDays(userAddDays)
                    .AddHours(userAddHours)
                    .AddMinutes(userAddMinutes);


                /**********start helper program************/
                //pass in the path of the helper program
                string pathOfHelperProgram = @".\TwitterBotDotNetHelper.exe";
                ProcessStartInfo startInfo = new ProcessStartInfo(pathOfHelperProgram);

                //send variables to helper program
                startInfo.Arguments = string.Format("{0} \"{1}\" {2} {3} {4} {5}", userInput, captionToTweet, filePath, newTime, newTime, newTime);

                Process.Start(startInfo);

                //print time of scheduled post
                Console.WriteLine("Your tweet will be published at " + newTime);
                Console.WriteLine("Your file path is " + filePath);

            }   //tweet media later


            //scrape data from a website and tweet that text
            else if (userInput == "5")
            {

                //create a list to hold each of the scraped elements
                //join each of the items with a ","
                //post new string to twitter


                Console.WriteLine("This is under construction, but has limited functionallity right now.");
                Console.WriteLine("Displays hunting season of all animals in kentucky right now.");
                //grab html from website

                //fire html loader
                HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = web.Load("https://app.fw.ky.gov/SeasonDates/Default.aspx");
                //grab html from home page
                var InSeason = doc.DocumentNode.SelectNodes("//div[@class='accordion-heading']").ToList();

                //loop through animals
                int index = 1;
                foreach (var gameAnimals in InSeason)
                {
                    Console.WriteLine($"{index}). {gameAnimals.InnerText}");
                    index++;
                }

                Console.WriteLine("These are the animals in season right now. Post to twitter?");
                string userResponse = Console.ReadLine().ToLower();
                if(userResponse == "yes" || userResponse == "y")
                {
                    //post a tweet

                    string textToTweet = Console.ReadLine(); //the text to be tweeted
                    Tweet.PublishTweet(textToTweet);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("scraped element/s posted");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("scraped element/s could not be posted");
                    Console.ResetColor();
                }

                
            }






















            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not an Option");

            }   //return error



            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Bot ended");
            Console.ResetColor();

            Console.ReadLine();
        }


    }
}
