
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
using HtmlAgilityPack;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace TwitterBotDotNet
{
    class Program
    {
        //NOTE: for mac select Run from the top menu bar,
        //      Run with > custom configuration
        //      make sure 'run in terminal' is checked
        //      Run

        //TODO: add a way to save user api keys to a file -database, maybe just an excel file

        //TODO: convert to a razor pages front end

        //TODO: convert post btc price to its own exe --or async it(all the external exe) somehow??

        //TODO: string interpolation



        static void Main(string[] args)
        {
            Console.WriteLine("Would you like to sign in with the default bot? Or add your own api keys?");
            Console.WriteLine("Enter DEFAULT or ADDKEYS");
            string loginChoice = Console.ReadLine().ToLower();
            Console.WriteLine($"\n");

            if (loginChoice == "default")
            {
                //login to tester bot "autobot....."
                DefaultLogin();
                MainLoop();
            }

            else if (loginChoice == "addkeys")
            {
                //prompt user to enter credentials
                UserLogin();
                MainLoop();
            }
            else
            {
                Console.WriteLine("User must choose a valid option, logging into default account.");
                DefaultLogin();
                MainLoop();
            }

            Console.WriteLine("The program is now closing in....");
            Thread.Sleep(600);
            Console.Beep();
            Console.WriteLine(3);
            Thread.Sleep(600);
            Console.Beep();
            Console.WriteLine(2);
            Thread.Sleep(600);
            Console.Beep();
            Console.WriteLine(1);
            Thread.Sleep(600);
            Console.Beep();

        }

        public static void DefaultLogin()
        {
            //read passwords from file here using your own file path

            string pathOfApiKeys = $@".{Path.DirectorySeparatorChar}api_keys.txt";
            //read file and put contents into array
            string[] allKeys = File.ReadAllLines(pathOfApiKeys);

            //default keys for test bot
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Login Successful");
                //validate which user is loged in
                Console.WriteLine($"{user} is signed in.");

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

        public static void UserLogin()
        {
            Console.WriteLine("You want to sign into your own account with api keys, enter them one at a time.");

            //set api keys
            Console.WriteLine("ApiKey");
            string ApiKey = Console.ReadLine();
            Console.WriteLine("ApiKeySecret");
            string ApiKeySecret = Console.ReadLine();
            Console.WriteLine("AccessToken");
            string AccessToken = Console.ReadLine();
            Console.WriteLine("AccessTokenSecret");
            string AccessTokenSecret = Console.ReadLine();


            // Set up your credentials (https://apps.twitter.com)
            Auth.SetUserCredentials(ApiKey, ApiKeySecret, AccessToken, AccessTokenSecret);

            //Login
            var user = User.GetAuthenticatedUser();
            if (user != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Login Successful");
                //validate which user is loged in
                Console.WriteLine($"{user} is signed in.");
                Console.ResetColor();
                Console.WriteLine("\n");

                //TODO: save login info
                //Console.WriteLine("\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could Not Login, Check Credentials or Internet Connection.");
                Console.ResetColor();
            }
        }

        public static void MainLoop()
        {
            while (Loop.keepGoing == "yes")
            {
                ProgramOptions();
                string userInput = Console.ReadLine();
                Console.WriteLine("\n");
                switch (userInput)
                {
                    case "1":
                        //publish tweet text on your timeline now
                        TweetText();
                        break;

                    case "2":
                        //publish media with a caption now
                        TweetMedia();
                        break;

                    case "3":
                        //tweet text later
                        ScheduleTweetText();
                        break;

                    case "4":
                        //tweet media later
                        ScheduleTweetMedia();
                        break;
                    case "5":
                        //scrape deal of the day at bestbuy
                        TweetBookOfTheDay();
                        break;

                    case "6":
                        //scrape data from a website and tweet that text now
                        TweetHuntingSeaon();
                        break;

                    case "7":
                        //post news headline from a website(engadget.com) (popular) now from scrapped website now
                        ScrapeAndPostRandomArticleFromEngadget();
                        break;

                    case "8":
                        //initialize scraper now and to run every (4 minutes)
                        IntervalTweetNews();
                        break;
                    case "9":
                        //post price of "BTC"
                        PostBtc();
                        break;
                    case "10":
                        //post stock prices
                        PostStocks();
                        break;

                    default:
                        //return error
                        ReturnError();
                        break;
                }
                CheckForExit();
                Loop.keepGoing = Console.ReadLine();
            }
        }

        public static void ProgramOptions()
        {
            //program options
            Console.WriteLine("\n");
            string[] UserOptions = { "Text Only Tweet",
                                    "Picture and Text Tweet",
                                    "Schedule Tweet for later",
                                    "Schedule Media Tweet for later",
                                    "(Scrape) Book of the day",
                                    "(Scrape) hunting season and tweet",
                                    "(Scrape) news headlines on engadget.com and tweet",
                                    "(interval) Initialize Scrape for news headlines on engadget.com and tweet results every 4 minutes",
                                    "(webClient)Post Price of BTC",
                                    "(webClient)Post stock prices"
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
        public static void TweetText()
        {
            PrintInstructions();
            string textToTweet = Console.ReadLine();
            Tweet.PublishTweet(textToTweet);
            CheckTwitter();
        }

        //publish media with a caption now
        public static void TweetMedia()
        {
            //promt user to pick a picture
            PrintInstructionsForPictures();

            //working with the picture files
            //must be in .jpg

            //file name of all pictures
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
            Console.ForegroundColor = ConsoleColor.DarkGray;
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
            Console.ForegroundColor = ConsoleColor.DarkGray; ;
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
        public static void ScheduleTweetText()
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
        public static void ScheduleTweetMedia()
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
            Console.ForegroundColor = ConsoleColor.DarkGray; ;
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
            Console.ForegroundColor = ConsoleColor.DarkGray; ;
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
        public static void TweetHuntingSeaon()
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
            List<string> animalHashtag = new List<string>();
            string genricHashtags = "#Kentucky #HuntingSeason #ProjectWebScrape #CSharp";
            string animalHash = "";
            foreach (var gameAnimals in InSeason)
            {
                //add each animal to a list and add each with a # to another list
                //get animal hastags
                animalHash = " #" + gameAnimals.InnerText;
                animalHashtag.Add(animalHash);
                //add animals to a list to be displayed
                animals.Add(gameAnimals.InnerText);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{index}). {gameAnimals.InnerText}");
                Console.ResetColor();
                index++;
            }

            //join hashtags
            string cleanHashtags = String.Join("", animalHashtag);
            //Console.WriteLine("should have # in front" + cleanHashtags);

            //join the list of animals into a string 
            string stringOfAnimals = String.Join(", ", animals);
            //Console.WriteLine(stringOfAnimals);

            //post
            Console.WriteLine("These are the animals in season right now. Post to twitter?");
            string userResponse = Console.ReadLine().ToLower();
            if (userResponse.ToLower() == "yes" || userResponse.ToLower() == "y")
            {
                //post a tweet
                Tweet.PublishTweet("Kentucky hunting season is open:  " + stringOfAnimals + "\n" + genricHashtags + cleanHashtags);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("scraped element/s posted");
                Console.ResetColor();
                CheckTwitter();
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

        //scrape bestbuy.com for deal of the day
        public static void TweetBookOfTheDay()
        {

            Console.WriteLine("Displays book of the day!");
            //grab html from website

            //fire html loader
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://bookoftheday.org/");

            //grab html from home page
            var Book = doc.DocumentNode.SelectSingleNode("//header/hgroup/h2");
            string bookOfTheDay = Book.InnerText;


            //set hashtag to first and last name of the author
            string[] getAuthor = bookOfTheDay.Split('&');
            getAuthor[1].ToString();
            string[] getAuthorWords = getAuthor[1].Split(' ');

            string cleanHashtag = "#Books #BookofTheDay #CSharp #ProjectWebScrape #";

            cleanHashtag += getAuthorWords[1];
            cleanHashtag += getAuthorWords[2];

            //handles an author using a middle name or miultiple authors
            //TODO: refactor later, has to be a better way
            if (getAuthorWords.Length == 4)
            {
                cleanHashtag += getAuthorWords[3];
            }
            if (getAuthorWords.Length == 5)
            {
                cleanHashtag += getAuthorWords[4];
            }
            if (getAuthorWords.Length == 6)
            {
                cleanHashtag += getAuthorWords[5];
            }
            if (getAuthorWords.Length == 7)
            {
                cleanHashtag += getAuthorWords[6];
            }
            if (getAuthorWords.Length == 8)
            {
                cleanHashtag += getAuthorWords[7];
            }

            Console.WriteLine($"The hashtag of the author's first and last name together is: {cleanHashtag}");
            Console.WriteLine("\n");

            //print clean title
            string cleanBookAndAuth = bookOfTheDay.Replace("&#8211;", "by");
            //Console.WriteLine(cleanBookAndAuth);

            //text to be posted to twitter
            Console.ForegroundColor = ConsoleColor.Blue;
            string textToTweet = "Book of the Day is: " + cleanBookAndAuth + "\n" + cleanHashtag;
            Console.WriteLine(textToTweet);
            Console.ResetColor();

            Tweet.PublishTweet(textToTweet);
            CheckTwitter();

        }

        //run news scrapper once, now
        public static void ScrapeAndPostRandomArticleFromEngadget()
        {
            //scrape data
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://www.engadget.com/");
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

            //default article to post
            string articleToPost = newsList[0];

            //Post a random article out of the top few articles
            Random randomArticle = new Random();
            articleToPost = newsList[randomArticle.Next(15)];
            //articleToPost = newsList[10];
            //print the random article in blue 
            Console.ForegroundColor = ConsoleColor.Blue;

            //get rid of weird &#039; thing in scraped articles
            string cleanArticle = articleToPost.Replace("&#039;", "");

            Console.WriteLine(cleanArticle);
            Console.ResetColor();


            //grab all uppercase letters of the new article and add that word as a hastag
            string[] split = cleanArticle.Split(' ');
            string[] listOfHashtags = new string[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                if (split[i].Any(char.IsUpper))
                {
                    listOfHashtags[i] = "#" + split[i] + " ";

                }
            }

            //join the list together
            string generatedHashTags = string.Join("", listOfHashtags);

            string cleanHashtags = generatedHashTags.Replace("’", "")
                                                    .Replace("‘", "")
                                                    .Replace("'", "")
                                                    .Replace(",", "")
                                                    .Replace("-", "");

            //TODO: LATER: turns the text into just the quote 
            //Goal: is to get rid of the spaces within a quote and post as a hashtag

            string engadgetHashtags = $"#Engadget #ProjectWebScrape #CSharp";
            Console.ForegroundColor = ConsoleColor.Green;
            string textToTweet = $"TOP STORY: { cleanArticle } \n { engadgetHashtags } { cleanHashtags }";
            Console.ResetColor();
            Console.WriteLine("\n");
            //shows dynamic hashtags
            Console.Write($"The Hashtags being sent with the tweet: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{ engadgetHashtags } { cleanHashtags }");
            Console.ResetColor();
            Console.WriteLine("\n");
            //shows what will be posted
            Console.Write($"Story to Publish: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(textToTweet);
            Console.ResetColor();
            Console.WriteLine("\n");

            //post the news story
            //this is here to post as soon as the program runs
            Tweet.PublishTweet(textToTweet);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("STORY posted");
            Console.ResetColor();
            Console.WriteLine("\n");


        }

        //initialize news scrapper, runs now and then every 4 minutes picks a random top article to post
        //automated hashtags by grabbing all capital words
        public static void IntervalTweetNews()
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

        //post current price of btc
        //make this a webhook -only post when a milestone is hit??
        public static void PostBtc()
        {

            //convert to a helper program
            while (true)
            {
                int timesPosted = 0;

                timesPosted++;

                Console.WriteLine($"Number of times the price of btc has posted in this instance is {timesPosted}");
                Console.WriteLine($"\n");
                Console.WriteLine("Program will run again in an hour");

                string json;
                decimal targetPrice;

                //start web client
                using (var web = new System.Net.WebClient())
                {
                    var url = @"https://api.coindesk.com/v1/bpi/currentprice.json";
                    json = web.DownloadString(url);
                }
                //parse into usable data
                dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                var currentPrice = Convert.ToDecimal(obj.bpi.USD.rate.Value);
                Console.WriteLine($"BTC: ${currentPrice}");

                

                //some math for additional information
                decimal remainder = currentPrice % 1000;
                decimal distanceToMilestone = 1000 - remainder;

                //find the next whole thousand dollar for a targetprice point
                targetPrice = (currentPrice - remainder) + 1000;

                Console.WriteLine($"${distanceToMilestone} away from {targetPrice}");
                Console.ForegroundColor = ConsoleColor.Blue;
                string textToTweet = $"The current price of BTC is: ${Convert.ToDouble(currentPrice)} \n #BTC #CryptoCurrency #Coindesk \n \n \n Powered by CoinDesk \n https://www.coindesk.com/price/bitcoin ";
                Console.ResetColor();
                //post special string when milestones are hit
                //but what if we start the program and the price is already 14000

                if (currentPrice > targetPrice)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    textToTweet = $"Bitcoin is above ${targetPrice} again! \n #BTC #CryptoCurrency #Coindesk \n Powered by CoinDesk \n https://www.coindesk.com/price/bitcoin ";
                    Console.ResetColor();
                    targetPrice += 1000;
                }


                





                Tweet.PublishTweet(textToTweet);
                Console.WriteLine("It is not time to post yet, the program will try again in 60 minutes...");
                Thread.Sleep(7200000);   //wait for 1 hour
            }//end of while loop
            
        }


        //A work in progress...
        public static void PostStocks()
        {
            
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

        public static void CheckTwitter()
        {
            Console.WriteLine("Would you like to check twitter to make sure? Select: Yes or No");
            string checkTwitter = Console.ReadLine();
            if (checkTwitter.ToLower() == "yes" || checkTwitter.ToLower() == "y")
            {
                Process.Start("http://www.twitter.com/@autoBot04768645");
            }
        }

    }//end of program class


}//end
