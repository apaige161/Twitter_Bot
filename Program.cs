
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
        //TODO: convert to a razor pages front end

        static void Main(string[] args)
        {
            Login();

            string keepGoing = "yes";

            while(keepGoing.ToLower() == "yes")
            {
                ProgramOptions();
                string userInput = Console.ReadLine();
                Console.WriteLine("\n");
                switch (userInput)
                {
                    case "1":
                        //publish tweet text on your timeline now
                        Option1();
                        break;

                    case "2":
                        //publish media with a caption now
                        Option2();
                        break;

                    case "3":
                        //tweet text later
                        Option3();
                        break;

                    case "4":
                        //tweet media later
                        Option4();
                        break;

                    case "5":
                        //scrape data from a website and tweet that text now
                        Option5();
                        break;

                    case "6":
                        //post news headline from a website(engadget.com) (popular) now from scrapped website now
                        Option6();
                        break;

                    case "7":
                        //initialize scraper now and to run every (4 minutes)
                        Option7();
                        break;

                    default:
                        //return error
                        ReturnError();
                        break;
                }
                CheckForExit();
                keepGoing = Console.ReadLine(); 
            }
        }

        public static void Login()
        {
            /***login***/

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Bot started");

            //read passwords from file here using your own file path

            string pathOfApiKeys = $@".{Path.DirectorySeparatorChar}api_keys.txt";
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
        }

        public static void ProgramOptions()
        {
            //program options
            Console.WriteLine("\n");
            string[] UserOptions = { "Text Only Tweet",
                                    "Picture and Text Tweet",
                                    "Schedule Tweet for later",
                                    "Schedule Media Tweet for later",
                                    "Scrape for what is in season and tweet",
                                    "Scrape for news headlines on engadget.com and tweet",
                                    "Initialize Scrape for news headlines on engadget.com and tweet results every day"
            };

            Console.WriteLine("Choose an option by typing the number");
            for (int i = 0; i < UserOptions.Length; i++)
            {
                Console.WriteLine(i + 1 + "). " + UserOptions[i]);
            } //display menu
            Console.WriteLine("\n");

        }

        public static void PrintInstructions()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("What would you like to say on twitter?");
            Console.ResetColor();
        }

        public static void PrintInstructionsForPictures()
        {
            //promt user to pick a picture
            Console.Write("INSTRUCTIONS: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Select the picture you want to post");
            Console.WriteLine("\n");
        }


        //publish tweet text on your timeline now
        public static void Option1()
        {
            PrintInstructions();
            string textToTweet = Console.ReadLine();
            Tweet.PublishTweet(textToTweet);
            CheckTwitter();
        }

        //publish media with a caption now
        public static void Option2()
        {
            //promt user to pick a picture
            PrintInstructionsForPictures();

            /******working with the picture files******/
            //must be in .jpg
            /******file name of all pictures******/
            string pathOfPics = $@".{Path.DirectorySeparatorChar}twitterImg";
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

            if (showAllPictureNames.ToLower() == "yes" || showAllPictureNames.ToLower() == "" || showAllPictureNames.ToLower() == "y")
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
            if (userSearch.ToLower() == "yes" || userSearch.ToLower() == "" || userSearch.ToLower() == "y")
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
            else if (userSearch.ToLower() == "no" || userSearch.ToLower() == "n")
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
            string filePath = pathOfPics + $@"{Path.DirectorySeparatorChar}" + fileNames[realUserChoice].ToString();

            //user input : picture caption
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("What would you like to say with your picture on twitter?");
            Console.ResetColor();
            //capture input
            string textToTweet = "Generic words, no user input";
            textToTweet = Console.ReadLine();
            Console.WriteLine("\n");



            /*********send picture tweet**********/
            //exception unhandled if no input
            byte[] file1 = File.ReadAllBytes(filePath);
            var media = Upload.UploadBinary(file1);
            Tweet.PublishTweet(textToTweet + " " + DateTime.Now, new PublishTweetOptionalParameters
            {
                Medias = new List<IMedia> { media }
            });

            CheckTwitter();
        }

        //schedule tweet text
        public static void Option3()
        {
            Console.WriteLine("You will post a tweet at a later date");
            Console.WriteLine("How many days do you want to wait?");
            int userAddDays = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("How many hours do you want to wait?");
            int userAddHours = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("How many minutes do you want to wait?");
            int userAddMinutes = Convert.ToInt32(Console.ReadLine());

            //get user info
            PrintInstructions();
            string textToTweet = Console.ReadLine();

            //add time to the current time

            //get current time
            DateTime currentTime = DateTime.Now;
            Console.WriteLine("Current time = {0}", currentTime);

            //add days, hours, minutes
            DateTime newTime = DateTime.Now
                .AddDays(userAddDays)
                .AddHours(userAddHours)
                .AddMinutes(userAddMinutes);


            /**********start helper program************/
            //pass in the path of the helper program
            string pathOfHelperProgram = $@".{Path.DirectorySeparatorChar}TwitterBotDotNetHelper.exe";

            ProcessStartInfo startInfo = new ProcessStartInfo(pathOfHelperProgram);

            int userInput = 3;

            //TODO: string interpolation
            //for whatever reason the compiler does not like string interpolation here, LATER: refactor
            startInfo.Arguments = string.Format("{0} \"{1}\" {2} {3} {4}", userInput, textToTweet, newTime, newTime, newTime);

            Process.Start(startInfo);

            //print time of scheduled post
            Console.WriteLine("Your tweet will be published at " + newTime);
        }

        //schedule media with caption to tweet
        public static void Option4()
        {
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
            PrintInstructionsForPictures();


            /******working with the picture files (must be in .jpg)******/

            /**file name of all pictures**/
            string pathOfPics = $@".{Path.DirectorySeparatorChar}twitterImg";
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
            if (showAllPictureNames.ToLower() == "yes" || showAllPictureNames.ToLower() == "" || showAllPictureNames.ToLower() == "y")
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
            if (userSearch.ToLower() == "yes" || userSearch.ToLower() == "" || userSearch.ToLower() == "y")
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
            else if (userSearch.ToLower() == "no" || userSearch.ToLower() == "n")
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
            string filePath = pathOfPics + $@"{Path.DirectorySeparatorChar}" + fileNames[realUserChoice].ToString();

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
            string pathOfHelperProgram = $@".{Path.DirectorySeparatorChar}TwitterBotDotNetHelper.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo(pathOfHelperProgram);
            int userInput = 4;

            //send variables to helper program
            startInfo.Arguments = string.Format("{0} \"{1}\" {2} {3} {4} {5}", userInput, captionToTweet, filePath, newTime, newTime, newTime);

            Process.Start(startInfo);

            //print time of scheduled post
            Console.WriteLine("Your tweet will be published at " + newTime);
            Console.WriteLine("Your file path is " + filePath);
        }

        //scrape ky.gov for animals in season once, now
        public static void Option5()
        {
            Console.WriteLine("Displays hunting season of all animals in kentucky right now.");
            //grab html from website

            //fire html loader
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://app.fw.ky.gov/SeasonDates/Default.aspx");
            //grab html from home page
            var InSeason = doc.DocumentNode.SelectNodes("//div[@class='accordion-heading']").ToList();

            //loop through animals
            int index = 1;
            List<string> animals = new List<string>();
            foreach (var gameAnimals in InSeason)
            {
                //add each animal to a list
                string gameWithSpace = gameAnimals.InnerText;
                animals.Add(gameWithSpace);
                Console.WriteLine($"{index}). {gameWithSpace}");
                index++;
            }

            //join the list of animals into a string 
            string stringOfAnimals = String.Join(", ", animals);
            Console.WriteLine(stringOfAnimals);

            //post
            Console.WriteLine("These are the animals in season right now. Post to twitter?");
            string userResponse = Console.ReadLine().ToLower();
            if (userResponse == "yes" || userResponse == "y")
            {
                //post a tweet
                Tweet.PublishTweet("Kentucky hunting season is open for:  " + stringOfAnimals);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("scraped element/s posted");
                Console.ResetColor();
            }
            else if (userResponse.ToLower() == "no" || userResponse.ToLower() == "n")
            {
                Console.WriteLine("Elements were not posted");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("scraped element/s could not be posted, please provide a proper response");
                Console.ResetColor();
            }
        }

        //run news scrapper once, now
        public static void Option6()
        {
            //scrape data
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.engadget.com/");
            //grab html from home page
            var NewsArticles = doc.DocumentNode.SelectNodes("//span[@class='th-underline']");

            //loop and print headlines
            int index = 1;
            List<string> newsList = new List<string>();
            foreach (var NewsArticle in NewsArticles)
            {
                //add each article to a list
                //trim the blank space
                string atricleText = NewsArticle.InnerText.Replace("\n", "").Trim();
                newsList.Add(atricleText);

                if (index < 15)
                    Console.WriteLine($"TOP STORY: {atricleText}");

                index++;
            }
            Console.WriteLine("\n");

            //post articles

            //TODO: - Done in scraper helper -  Post a random article out of the top few articles

            //Print hashtags to be posted
            string input = newsList[0];
            string firstWordOfArticle = input.Substring(0, input.IndexOf(" ")); // Result is "first word of article"
            Console.Write($"The Hashtags being sent with the tweet: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"#{firstWordOfArticle} #Engadget #ProjectWebScrape");
            Console.ResetColor();

            //post articles
            Console.WriteLine("Would you like to post the top news story?");
            string postArticle = Console.ReadLine().ToLower();

            if (postArticle == "yes" || postArticle == "y")
            {
                Console.WriteLine($"{ newsList[0] }");
                //post a tweet
                Tweet.PublishTweet($"TOP STORY: { newsList[0] } #Engadget and some words");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("scraped element/s posted");
                Console.ResetColor();
            }
        }

        //initialize news scrapper, runs now and then every 4 minutes picks a random top article to post
        //automated hashtags by grabbing all capital words
        public static void Option7()
        {
            Console.WriteLine("You have initialized the news scraper, it will look for the top story every 4 minutes and post it.");
            Console.WriteLine("The scraper will randomly choose 1 of the top 14 articles scraped.");
            Console.WriteLine("The hashtags will be generated by the first word in the article and also all captialized words.");

            //start helper program

            //pass in the path of the helper program
            string pathOfHelperProgram = $@".{Path.DirectorySeparatorChar}TwitterBotDotNetScraperHelper.exe";

            ProcessStartInfo startInfo = new ProcessStartInfo(pathOfHelperProgram);

            Process.Start(startInfo);

            //print time of scheduled post
            Console.WriteLine("Your tweet will be published every 4 minutes as long as the program is running");
        }

        public static void ReturnError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Not an Option");
        }

        public static void CheckForExit()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Continue? YES or NO");
            Console.ResetColor();
        }

        //opens chrome and goes directly to twitter
        public static void CheckTwitter()
        {
            //check twitter
            Console.WriteLine("Would you like to check twitter to make sure? Select: Yes or No");
            Console.WriteLine("Only works for windows, sorry Apple people :(");
            string checkTwitter = Console.ReadLine();
            if (checkTwitter.ToLower() == "yes" || checkTwitter.ToLower() == "y")
            {
                //opens chrome to twitter page
                // url's are not considered documents. They can only be opened
                // by passing them as arguments.
                Process.Start("Chrome.exe", "http://www.twitter.com/@autoBot04768645");
            }
        }
    }


}
