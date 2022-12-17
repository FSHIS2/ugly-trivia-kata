using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        private readonly List<string> players = new List<string>();

        private readonly int[] places = new int[6];
        private readonly int[] purses = new int[6];

        private readonly bool[] inPenaltyBox = new bool[6];

        private readonly LinkedList<string> popQuestions = new LinkedList<string>();
        private readonly LinkedList<string> scienceQuestions = new LinkedList<string>();
        private readonly LinkedList<string> sportsQuestions = new LinkedList<string>();
        private readonly LinkedList<string> rockQuestions = new LinkedList<string>();

        private int currentPlayer;
        private bool isGettingOutOfPenaltyBox;

        public Game() {
            CreateQuestions();
        }

        private void CreateQuestions() {
            for (var i = 0; i < 50; i++) {
                CreateQuestionCategories(i);
            }
        }

        private void CreateQuestionCategories(int i) {
            popQuestions.AddLast(CreatePopQuestion(i));
            scienceQuestions.AddLast(CreateScienceQuestion(i));
            sportsQuestions.AddLast(CreateSportsQuestion(i));
            rockQuestions.AddLast(CreateRockQuestion(i));
        }

        private static string CreatePopQuestion(int i) {
            return "Pop Question " + i;
        }

        private static string CreateScienceQuestion(int i) {
            return "Science Question " + i;
        }

        private static string CreateSportsQuestion(int i) {
            return "Sports Question " + i;
        }

        public string CreateRockQuestion(int index)
        {
            return "Rock Question " + index;
        }

        public bool Add(string playerName)
        {
            players.Add(playerName);
            places[HowManyPlayers()] = 0;
            purses[HowManyPlayers()] = 0;
            inPenaltyBox[HowManyPlayers()] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + players.Count);
            return true;
        }

        public int HowManyPlayers()
        {
            return players.Count;
        }

        public void Roll(int roll)
        {
            Console.WriteLine(players[currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (inPenaltyBox[currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(players[currentPlayer] + " is getting out of the penalty box");
                    places[currentPlayer] = places[currentPlayer] + roll;
                    if (places[currentPlayer] > 11) places[currentPlayer] = places[currentPlayer] - 12;

                    Console.WriteLine(players[currentPlayer]
                            + "'s new location is "
                            + places[currentPlayer]);
                    Console.WriteLine("The category is " + CurrentCategory());
                    AskQuestion();
                }
                else
                {
                    Console.WriteLine(players[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                places[currentPlayer] = places[currentPlayer] + roll;
                if (places[currentPlayer] > 11) places[currentPlayer] = places[currentPlayer] - 12;

                Console.WriteLine(players[currentPlayer]
                        + "'s new location is "
                        + places[currentPlayer]);
                Console.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }
        }

        private void AskQuestion() {
            SelectQuestionCategory();
        }

        private void SelectQuestionCategory() {
            if (CurrentCategory() == "Pop") {
                RemoveFromPop();
            }

            if (CurrentCategory() == "Science") {
                RemoveFromScience();
            }

            if (CurrentCategory() == "Sports") {
                RemoveFromSports();
            }

            if (CurrentCategory() == "Rock") {
                RemoveFromRock();
            }
        }

        private void RemoveFromRock() {
            Console.WriteLine(rockQuestions.First());
            rockQuestions.RemoveFirst();
        }

        private void RemoveFromSports() {
            Console.WriteLine(sportsQuestions.First());
            sportsQuestions.RemoveFirst();
        }

        private void RemoveFromScience() {
            Console.WriteLine(scienceQuestions.First());
            scienceQuestions.RemoveFirst();
        }

        private void RemoveFromPop() {
            Console.WriteLine(popQuestions.First());
            popQuestions.RemoveFirst();
        }

        private string CurrentCategory()
        {
            if (places[currentPlayer] == 0) return "Pop";
            if (places[currentPlayer] == 4) return "Pop";
            if (places[currentPlayer] == 8) return "Pop";
            if (places[currentPlayer] == 1) return "Science";
            if (places[currentPlayer] == 5) return "Science";
            if (places[currentPlayer] == 9) return "Science";
            if (places[currentPlayer] == 2) return "Sports";
            if (places[currentPlayer] == 6) return "Sports";
            if (places[currentPlayer] == 10) return "Sports";
            return "Rock";
        }

        public bool WasCorrectlyAnswered()
        {
            if (inPenaltyBox[currentPlayer])
            {
                if (isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    purses[currentPlayer]++;
                    Console.WriteLine(players[currentPlayer]
                            + " now has "
                            + purses[currentPlayer]
                            + " Gold Coins.");

                    var winner = DidPlayerWin();
                    currentPlayer++;
                    if (currentPlayer == players.Count) currentPlayer = 0;

                    return winner;
                }
                else
                {
                    currentPlayer++;
                    if (currentPlayer == players.Count) currentPlayer = 0;
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Answer was corrent!!!!");
                purses[currentPlayer]++;
                Console.WriteLine(players[currentPlayer]
                        + " now has "
                        + purses[currentPlayer]
                        + " Gold Coins.");

                var winner = DidPlayerWin();
                currentPlayer++;
                if (currentPlayer == players.Count) currentPlayer = 0;

                return winner;
            }
        }

        public bool WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(players[currentPlayer] + " was sent to the penalty box");
            inPenaltyBox[currentPlayer] = true;

            currentPlayer++;
            if (currentPlayer == players.Count) currentPlayer = 0;
            return true;
        }


        private bool DidPlayerWin()
        {
            return purses[currentPlayer] != 6;
        }
    }

}
