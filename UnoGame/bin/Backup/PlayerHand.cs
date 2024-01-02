// using UnoGame.GameObject;
// using System;
// using System.Collections.Generic;
//
// namespace UnoGame.GameLogic
// {
//     public class PlayerHand
//     {
//         private List<Card> hand;
//
//         public PlayerHand()
//         {
//             hand = new List<Card>();
//         }
//
//         public void AddCardToHand(Card card)
//         {
//             hand.Add(card);
//         }
//
//         public void RemoveCardFromHand(Card card)
//         {
//             hand.Remove(card);
//         }
//
//         public List<Card> GetCardsInHand()
//         {
//             return hand;
//         }
//
//         public void SortHand()
//         {
//             // Implement sorting logic for the hand, e.g., by color or value.
//         }
//
//         public bool HasCard(Card card)
//         {
//             return hand.Contains(card);
//         }
//
//         public Card DrawCardFromHand(Card card)
//         {
//             if (hand.Contains(card))
//             {
//                 // Remove the card from the hand
//                 hand.Remove(card);
//                 return card; // Return the drawn card
//             }
//             else
//             {
//                 // The specified card is not in the hand, return null or handle the error as needed
//                 return null;
//             }
//         }
//
//         public bool HasEmptyHand()
//         {
//             return hand.Count == 0; // Check if the hand is empty
//         }
//     }
// }