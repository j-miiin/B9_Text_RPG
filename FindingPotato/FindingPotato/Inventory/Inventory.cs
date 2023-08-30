﻿using FindingPotato.Character;
using FindingPotato.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FindingPotato.Inventory
{
    internal class InventoryClass
    {
        private static readonly Dictionary<ItemType, string> EffectDictionary = new Dictionary<ItemType, string>
        {
            { ItemType.StrengthPotion, "공격력 (1회)" },
            { ItemType.HealthPotion, "체력 회복" },
            { ItemType.Weapon, "공격력" },
            { ItemType.Armor, "방어력" }
        };


        public List<IItem> InventoryItems;
        public InventoryClass()
        {
             InventoryItems = new List<IItem>();
        }

        // isManagement는 GameManager의 ItemManagement에서 호출되었을 때  true  전달 
        public static void PrintTitle(bool isManagement)
        { 
            if (isManagement)
            {
                Console.SetCursorPosition(18, 1);
                Console.Write(" - 아이템 장착 및 소모");
                Console.SetCursorPosition(0, 0);
            }
            Console.WriteLine("  --------------");
            Console.WriteLine(" |   인벤토리   |");
            Console.WriteLine("  --------------");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            if (!isManagement) Extension.TypeWriting("  보유 중인 아이템을 관리할 수 있습니다.");
            else Console.WriteLine("  보유 중인 아이템을 관리할 수 있습니다.");
            Console.ResetColor();
        }

        public void ShowOptions()       
        {

            switch (InventoryItems.Count)
            {
                case 0:
                    break;
                case 1:
                    Extension.ColorWriteLine("\n1. 해당 아이템 장착 및 소모");
                    break;
                default:
                    Extension.ColorWriteLine($"\n1 ~ {InventoryItems.Count}. 해당 아이템 장착 및 소모");
                    break;
            }
            Extension.ColorWriteLine("\n0. 나가기");
        }

        public void PrintItemList(bool isManagement)
        {

            Console.SetCursorPosition(0, 6);
            Console.WriteLine($"◇----------◇----------◇----------◇----------◇----------◇----------◇----------◇----------◇----------◇----------\n");
            if (InventoryItems == null || InventoryItems.Count == 0)
            {
                Extension.ColorWriteLine("\n                                            보유 중인 아이템이 없습니다.\n", ConsoleColor.Black, ConsoleColor.DarkGray);
            }
            else
            {
                foreach (IItem item in InventoryItems)
                {
                    PrintItemInfo(item);
                }

                Console.SetCursorPosition(0, 8);
                // 리스트 앞 기호 출력
                for (int i = 1; i < InventoryItems.Count + 1; i++)
                {
                    Console.WriteLine(isManagement ? $" {i}." : " -");
                }
            }
            Console.WriteLine($"\n◇----------◇----------◇----------◇----------◇----------◇----------◇----------◇----------◇----------◇----------");

        }


        private static void PrintItemInfo(IItem item)
        {
            int initialLine = Console.CursorTop;
            Console.Write("                     |                    |                    |                                        |                    "); // 간격 20 | 20 | 20 | 40 | 20

            if (item is IEquipable equipable) // 장착 중인 아이템의 경우 장착 표시
            {
                if (equipable.IsEquipped) { WriteAtPosition("[E]", 3, initialLine); }
            }

            WriteAtPosition(item.Name, 7, initialLine);
            WriteAtPosition(item.Type.ToString(), 26, initialLine);

            if (EffectDictionary.TryGetValue(item.Type, out var effect))
            {
                WriteAtPosition($"{effect} + {item.Effect}", 46, initialLine);
            }

            WriteAtPosition(item.Desc, 66, initialLine);

            if (item is IConsumable consumable) // 소모품 개수 표시
            {
                WriteAtPosition($"보유 중: {consumable.Quantity} 개", 106, initialLine  );
            }

            Console.WriteLine();
        }

        private static void WriteAtPosition(string text, int position, int line)
        {
            Console.SetCursorPosition(position, line);
            Console.Write(text);
        }
    }
}
