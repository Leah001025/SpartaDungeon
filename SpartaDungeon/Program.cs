using System;
using System.Collections.Generic;
using System.Linq;

namespace SpartaDungeon
{
    // 플레이어 클래스
    public class Player
    {
        public string playerName { get; set; } // 플레이어 이름
        public string playerJob { get; set; } // 플레이어 직업
        public int playerLevel { get; set; } // 플레이어 레벨
        public int baseAtk { get; set; } // 플레이어 기본 공격력
        public int addAtk { get; set; } // 플레이어 추가 공격력
        public int baseDef { get; set; } // 플레이어 기본 방어력
        public int addDef { get; set; } // 플레이어 추가 방어력
        public int playerHp { get; set; } // 플레이어 체력
        public int playerGold { get; set; } // 플레이어 소지 골드

        public Player(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            playerName = name;
            playerJob = job;
            playerLevel = level;
            baseAtk = atk;
            baseDef = def;
            playerHp = hp;
            playerGold = gold;
        }
    }

    // 아이템 데이터 클래스
    public class ItemData
    {
        public bool IsEquipped { get; set; } // 장비 착용 여부
        public int ItemNumber { get; set; } // 장비 번호
        public string ItemName { get; set; } // 아이템 이름
        public string ItemType { get; set; } // 아이템 타입
        public int ItemStat { get; set; } // 아이템 스탯
        public string ItemInfo { get; set; } // 아이템 정보

        public ItemData(bool isEquipped, int number, string name, string type, int stat, string info)
        {
            IsEquipped = isEquipped;
            ItemNumber = number;
            ItemName = name;
            ItemType = type;
            ItemStat = stat;
            ItemInfo = info;
        }

        // 아이템 착용/해제 메서드
        public bool SetEquipped()
        {
            IsEquipped = !IsEquipped;
            return IsEquipped;
        }
    }

    // 상점 아이템 클래스
    public class ShopItemData
    {
        public bool IsBought { get; set; } // 아이템 구매 여부
        public int ShopItemNumber { get; set; } // 상점 아이템 번호
        public string ShopItemName { get; set; } // 상점 아이템 이름
        public string ShopItemType { get; set; } // 상점 아이템 타입
        public int ShopItemStat { get; set; } // 상점 아이템 스탯
        public string ShopItemInfo { get; set; } // 상점 아이템 정보
        public int ShopItemPrice { get; set; } // 상점 아이템 가격

        public ShopItemData(int shopItemNumber, string shopItemName, string shopItemType, int shopItemStat, string shopItemInfo, int shopItemPrice)
        {
            ShopItemNumber = shopItemNumber;
            ShopItemName = shopItemName;
            ShopItemType = shopItemType;
            ShopItemStat = shopItemStat;
            ShopItemInfo = shopItemInfo;
            ShopItemPrice = shopItemPrice;
        }
    }

    internal class Program
    {
        // 플레이어 및 매니저 초기화
        static Player player;
        static InventoryManager inventoryManager = new InventoryManager();
        static ShopManager shopManager = new ShopManager();

        static void Main(string[] args)
        {
            Console.Title = "스파르타 던전";
            Console.WriteLine("스파르타 던전 월드에 오신 것을 환영합니다.");
            Console.WriteLine("던전에 입장 하기 전, 당신의 이름을 알려주세요. ");
            Console.Write(">>");
            string playerName = Console.ReadLine();
            Console.Clear();

            player = new Player(playerName, "전사", 1, 10, 5, 100, 1000);

            ShowMainMenu();
        }
        ///////////////////////////////////////////////// 인벤토리 시작 /////////////////////////////////////////////////////////////
        // 인벤토리 관리 클래스
        public class InventoryManager
        {
            private List<ItemData> items;

            public InventoryManager()
            {
                items = new List<ItemData>();
                items.AddRange(new ItemData[] {
                    new ItemData(false, 1, "낡은 천 갑옷", "방어구", +1, "낡은 천으로 만들어진 갑옷"),
                    new ItemData(false, 2, "질긴 가죽 갑옷", "방어구", +2, "질긴 가죽으로 만들어진 갑옷"),
                    new ItemData(false, 3, "단단한 철 갑옷", "방어구", +3, "철로 만들어진 단단한 갑옷"),

                    new ItemData(false, 4, "녹슨 검", "무기", +1, "곧이라도 부러질 것 같은 녹이 슨 검"),
                    new ItemData(false, 5, "낡은 검", "무기", +2, "그럭저럭 쓸만한 검"),
                    new ItemData(false, 6, "날카로운 검", "무기", +3, "무엇이든 다 벨 수 있을 것 같은 검")
                });
            }

            // 아이템 반환
            public List<ItemData> GetItems()
            {
                return items;
            }

            // 아이템 추가
            public void AddItem(ItemData newItem)
            {
                int maxItemNumber = items.Count > 0 ? items.Max(item => item.ItemNumber) : 0; // 가장 높은 아이템 번호 찾기
                newItem.ItemNumber = maxItemNumber + 1; // 아이템들 사이에서 가장 높은 번호 + 1
                items.Add(newItem); // 인벤토리에 추가
            }

        }

        // 인벤토리 보여주기
        static void DisplayInventory(InventoryManager inventoryManager)
        {
            Console.Clear();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("[아이템 목록]\n");

            Console.WriteLine(" No. | 상태 |      이름      |  타입  | 스탯 ");
            Console.WriteLine("-----------------------------------------------");

            if (inventoryManager.GetItems().Count > 0)
            {
                foreach (var item in inventoryManager.GetItems())
                {
                    string equipped = item.IsEquipped ? "[E]" : "[ ]";
                    string statType = item.ItemType == "방어구" ? "방어력" : "공격력";
                    string formattedItemNumber = $"{item.ItemNumber,-2}";

                    Console.WriteLine($"{formattedItemNumber} | {equipped,-4}| {item.ItemName,-18}| {item.ItemType,-5}| {statType} + {item.ItemStat,-3} | {item.ItemInfo}");
                }
            }

            else
            {
                Console.WriteLine("보유 중인 아이템이 없습니다.");
            }
        }
        ///////////////////////////////////////////////// 인벤토리 끝 //////////////////////////////////////////////////////////////



        ////////////////////////////////////////////////// 상점 시작 ///////////////////////////////////////////////////////////////
        // 상점 관리 클래스
        public class ShopManager
        {
            private List<ShopItemData> items;

            public ShopManager()
            {
                items = new List<ShopItemData>();
                items.AddRange(new ShopItemData[] {
                    new ShopItemData(1, "낡은 천 갑옷", "방어구", +1, "낡은 천으로 만들어진 갑옷", 400),
                    new ShopItemData(2, "질긴 가죽 갑옷", "방어구", +2, "질긴 가죽으로 만들어진 갑옷",600),
                    new ShopItemData(3, "단단한 철 갑옷", "방어구", +3, "철로 만들어진 단단한 갑옷", 800),
                    new ShopItemData(4, "녹슨 검", "무기", +1, "곧이라도 부러질 것 같은 녹이 슨 검", 400),
                    new ShopItemData(5, "낡은 검", "무기", +2, "그럭저럭 쓸만한 검", 600),
                    new ShopItemData(6, "날카로운 검", "무기", +3, "무엇이든 다 벨 수 있을 것 같은 검", 800)
                });
            }

            // 상점 아이템 반환
            public List<ShopItemData> GetItems()
            {
                return items;
            }
        }

        // 상점 보여주기
        static void DisplayShop(ShopManager shopManager)
        {
            Console.Clear();
            Console.WriteLine("[상점]");
            Console.WriteLine("[필요한 아이템을 얻을 수 있는 상점 입니다.]\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.playerGold} G\n");

            Console.WriteLine("[아이템 목록]");
            Console.WriteLine("No.|      이름      |  타입  | 스탯 |   가격   ");
            Console.WriteLine("-----------------------------------------------");

            foreach (var item in shopManager.GetItems())
            {
                string priceInfo = item.IsBought ? "구매 완료" : $"{item.ShopItemPrice} G";

                Console.WriteLine($"{item.ShopItemNumber,-1} | {item.ShopItemName,-16} | {item.ShopItemType,-5} | {item.ShopItemStat,-4} | {priceInfo,-9}");
            }
        }

        ///////////////////////////////////////////////////상점 끝////////////////////////////////////////////////////////////////



        ////////////////////////////////////////////////메인 메뉴 시작/////////////////////////////////////////////////////////////
        static void ShowMainMenu()
        {
            int MainMenu = 0;

            Console.WriteLine("스파르타 마을에 오신 것을 환영합니다.");
            Console.WriteLine("던전에 입장하기 전, 원하시는 행동을 선택해주세요.");
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 휴식하기");
            Console.WriteLine("5. 저장하기");
            Console.WriteLine("6. 종료하기");
            Console.Write(">>");

            bool validInput = false;
            while (!validInput)
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out MainMenu) && MainMenu >= 1 && MainMenu <= 6)
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 1 ~ 6번 중에서 선택하세요:");
                    Console.Write(">>");
                }
            }

            bool isRunning = true;

            while (isRunning)
            {
                switch (MainMenu)
                {
                    case 1:

                        int addAtk = 0;
                        int addDef = 0;

                        Console.Clear();
                        Console.WriteLine("[상태창]\n");
                        Console.WriteLine($"플레이어 이름: {player.playerName}");
                        Console.WriteLine($"직업: {player.playerJob}");
                        Console.WriteLine($"레벨: {player.playerLevel}");

                        foreach (var item in inventoryManager.GetItems())
                        {
                            if (item.IsEquipped)
                            {
                                if (item.ItemType == "무기")
                                {
                                    addAtk += item.ItemStat;
                                }
                                else if (item.ItemType == "방어구")
                                {
                                    addDef += item.ItemStat;
                                }
                            }
                        }

                        Console.WriteLine($"공격력 : {player.baseAtk - addAtk} (+{addAtk})");
                        Console.WriteLine($"방어력 : {player.baseDef - addDef} (+{addDef})");
                        Console.WriteLine($"체력: {player.playerHp}");
                        Console.WriteLine($"소지 골드: {player.playerGold} G");
                        Console.WriteLine("-----------------------");

                        Console.WriteLine(" ");
                        Console.WriteLine("1. 나가기");
                        Console.Write(">>");
                        string input = Console.ReadLine();
                        if (input == "1")
                        {
                            Console.Clear();
                            ShowMainMenu();
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                            Console.Write(">>");
                        }
                        Console.Clear();
                        break;
                    case 2: // 인벤토리
                        DisplayInventory(inventoryManager);

                        Console.WriteLine(" ");
                        Console.WriteLine("1. 아이템 착용 또는 해제");
                        Console.WriteLine("2. 이전 화면으로 돌아가기");
                        Console.Write(">>");
                        input = Console.ReadLine();
                        if (input == "1")
                        {
                            Console.WriteLine("착용하거나 해제 할 아이템 번호를 입력하세요:");
                            Console.Write(">>");
                            string itemInput = Console.ReadLine();

                            if (int.TryParse(itemInput, out int itemNumber) && itemNumber >= 1 && itemNumber <= inventoryManager.GetItems().Count)
                            {
                                ItemData selectedItem = inventoryManager.GetItems().Find(item => item.ItemNumber == itemNumber);

                                if (selectedItem != null)
                                {
                                    ItemData equippedItem = inventoryManager.GetItems().Find(item => item.IsEquipped && item.ItemType == selectedItem.ItemType);
                                    if (equippedItem != null && equippedItem != selectedItem) // 기존 장착된 아이템이 선택된 아이템과 같지 않으면 해제
                                    {
                                        equippedItem.SetEquipped();
                                        if (equippedItem.ItemType == "무기")
                                        {
                                            player.baseAtk -= equippedItem.ItemStat;
                                        }
                                        else if (equippedItem.ItemType == "방어구")
                                        {
                                            player.baseDef -= equippedItem.ItemStat;
                                        }
                                    }

                                    bool wasEquipped = selectedItem.IsEquipped;
                                    bool isEquippedNow = selectedItem.SetEquipped();

                                    Console.Clear();
                                    DisplayInventory(inventoryManager);
                                    string equippedStatus = isEquippedNow ? "[E]" : "[ ]";

                                    if (isEquippedNow)
                                    {
                                        // 아이템 착용 시 플레이어 스탯 업데이트
                                        if (selectedItem.ItemType == "무기")
                                        {
                                            player.baseAtk += selectedItem.ItemStat;
                                        }
                                        else if (selectedItem.ItemType == "방어구")
                                        {
                                            player.baseDef += selectedItem.ItemStat;
                                        }

                                        // 플레이어 스탯이 변경되었다는 문구 출력
                                        Console.WriteLine($"{selectedItem.ItemName}이(가) {equippedStatus}으로 설정되었습니다.");
                                        Console.WriteLine($"플레이어의 {(selectedItem.ItemType == "방어구" ? "방어력" : "공격력")}이 {selectedItem.ItemStat} 추가되었습니다. (현재 공격력: {player.baseAtk}, 현재 방어력: {player.baseDef})");
                                        Console.Write(">>");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        if (selectedItem.ItemType == "무기")
                                        {
                                            player.baseAtk -= selectedItem.ItemStat;
                                        }
                                        else if (selectedItem.ItemType == "방어구")
                                        {
                                            player.baseDef -= selectedItem.ItemStat;
                                        }

                                        Console.WriteLine($"{selectedItem.ItemName}이(가) {equippedStatus}으로 설정되었습니다.");
                                        Console.WriteLine($"플레이어의 {(selectedItem.ItemType == "방어구" ? "방어력" : "공격력")}이 {selectedItem.ItemStat} 감소했습니다. (현재 공격력: {player.baseAtk}, 현재 방어력: {player.baseDef})");
                                        Console.Write(">>");
                                        Console.ReadLine();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("해당 번호의 아이템이 없습니다. 다시 입력해주세요.");
                                    Console.Write(">>");
                                }
                            }
                            else
                            {
                                Console.WriteLine("잘못된 입력이거나 해당 번호의 아이템이 없습니다. 다시 입력해주세요.");
                                Console.Write(">>");
                            }
                        }
                        else if (input == "2")
                        {
                            Console.Clear();
                            ShowMainMenu();
                            isRunning = false;
                        }
                        break;


                    case 3: // 상점 기능 실행
                        while (true)
                        {
                            DisplayShop(shopManager);
                            Console.WriteLine("1. 아이템 구매");
                            Console.WriteLine("2. 이전 화면으로 돌아가기");
                            Console.Write(">>");
                            input = Console.ReadLine();

                            if (input == "1")
                            {
                                Console.WriteLine("구매 하시고 싶은 아이템 번호를 입력하세요:");
                                Console.Write(">>");
                                string itemInput = Console.ReadLine();
                                if (int.TryParse(itemInput, out int shopChoice) && shopChoice >= 1 && shopChoice <= shopManager.GetItems().Count)
                                {
                                    ShopItemData selectedItem = shopManager.GetItems().Find(item => item.ShopItemNumber == shopChoice);

                                    if (selectedItem != null && !selectedItem.IsBought)
                                    {
                                        if (player.playerGold >= selectedItem.ShopItemPrice)
                                        {
                                            // 아이템 구매 완료 처리
                                            selectedItem.IsBought = true;

                                            // 골드 차감
                                            player.playerGold -= selectedItem.ShopItemPrice;

                                            // 아이템을 인벤토리에 추가
                                            inventoryManager.AddItem(new ItemData(false, selectedItem.ShopItemNumber, selectedItem.ShopItemName, selectedItem.ShopItemType, selectedItem.ShopItemStat, selectedItem.ShopItemInfo));

                                            // 상점을 다시 표시하여 변경된 사항 반영
                                            DisplayShop(shopManager);

                                            Console.WriteLine($"{selectedItem.ShopItemName}을(를) 구매했습니다.");
                                            Console.WriteLine($"보유한 골드: {player.playerGold} G");

                                            Console.ReadLine();
                                            Console.Clear();
                                        }
                                        else
                                        {
                                            Console.WriteLine("골드가 부족합니다.");
                                            Console.ReadLine();
                                            Console.Clear();
                                        }
                                    }
                                    else if (selectedItem != null && selectedItem.IsBought)
                                    {
                                        Console.WriteLine("이미 구매한 아이템입니다.");
                                        Console.ReadLine();
                                        Console.Clear();
                                    }
                                    else
                                    {
                                        Console.WriteLine("잘못된 입력이거나 해당 번호의 아이템이 없습니다. 다시 입력해주세요.");
                                        Console.ReadLine();
                                        Console.Clear();
                                    }
                                }
                            }
                            else if (input == "2")
                            {
                                Console.Clear();
                                ShowMainMenu(); // 루프를 빠져나와 메인 메뉴로 돌아감
                                isRunning = false;
                            }
                        }
                        break;

                    case 4: // 휴식하기 기능 실행
                        Console.WriteLine("[휴식하기]");
                        Console.WriteLine("체력을 회복합니다. (비용: 500 G)");

                        Console.WriteLine("[보유 골드]");
                        Console.WriteLine($"{player.playerGold} G\n");

                        if (player.playerGold >= 500)
                        {
                            Console.WriteLine("1. 휴식하기");
                            Console.WriteLine("2. 나가기");
                            Console.Write(">>");
                            string choice = Console.ReadLine();

                            switch (choice)
                            {
                                case "1":
                                    player.playerHp += 100;
                                    player.playerHp = Math.Min(player.playerHp, 100); // 체력은 100을 초과하지 않도록 설정

                                    player.playerGold -= 500; // 휴식 비용 차감

                                    Console.WriteLine("휴식을 완료했습니다.");
                                    Console.WriteLine($"체력이 회복되었습니다. 현재 체력: {player.playerHp}");
                                    Console.ReadLine();
                                    Console.Clear();
                                    ShowMainMenu();
                                    isRunning = false;
                                    break;
                                case "2":
                                    Console.Clear();
                                    ShowMainMenu();
                                    isRunning = false;
                                    break;
                                default:
                                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                                    Console.ReadLine();
                                    Console.Write(">>");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Gold가 부족합니다.");
                            Console.ReadLine();
                            Console.Clear();
                            ShowMainMenu();
                            isRunning = false;
                        }
                        break;
                    case 5:
                        // 저장 기능 실행
                        break;
                    case 6: // 프로그램 종료
                        Console.WriteLine("프로그램을 종료합니다. 다음에 또 이용해주세요!");
                        Environment.Exit(0); 
                        break;
                    default:
                        break;
                }
            }
        }
        ////////////////////////////////////////////////메인 메뉴 끝/////////////////////////////////////////////////////////////
    }
}
