using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using System.IO;

public class ASM_MN : Singleton<ASM_MN>
{
    public List<Region> listRegion = new List<Region>();
    public List<Players> listPlayer = new List<Players>();




    private void Start()
    {
        createRegion();
        AddPlayer();
    }
    void AddPlayer()
    {
        listPlayer.Add(new Players(0, "Nguyen Van A", 1500, 0));
        listPlayer.Add(new Players(1, "Nguyen Van B", 300, 1));
        listPlayer.Add(new Players(2, "Nguyen Van C", 700, 2));
        listPlayer.Add(new Players(3, "Nguyen Van D", 1200, 3));
        listPlayer.Add(new Players(4, "Nguyen Van E", 50, 4));
        listPlayer.Add(new Players(5, "Nguyen Van F", 800, 0));
        listPlayer.Add(new Players(6, "Nguyen Van G", 200, 1));
        listPlayer.Add(new Players(7, "Nguyen Van H", 600, 2));
    }

    public void createRegion()
    {
        listRegion.Add(new Region(0, "VN"));
        listRegion.Add(new Region(1, "VN1"));
        listRegion.Add(new Region(2, "VN2"));
        listRegion.Add(new Region(3, "JS"));
        listRegion.Add(new Region(4, "VS"));
    }

    public string calculate_rank(int score)
    {
        // sinh viên viết tiếp code ở đây
        if (score >= 0 && score < 100)
        {
            return "Đồng";
        }
        else if (score >= 100 && score < 500)
        {
            return "Bạc";
        }
        else if (score >= 500 && score < 1000)
        {
            return "Vàng";
        }
        else if (score >= 1000)
        {
            return "Kim cương";
        }
        return null;
    }

    public void YC1()
    {
        // sinh viên viết tiếp code ở đây
        ScoreKeeper scoreKeeper = ScoreKeeper.Instance;
        int ID = scoreKeeper.GetID();
        string Name = scoreKeeper.GetUserName();
        int Score = scoreKeeper.GetScore();
        int Region = scoreKeeper.GetIDregion();

        Players player = new Players(ID, Name, Score, Region);
        listPlayer.Add(player);
        
    }
    public void YC2()
    {
        // sinh viên viết tiếp code ở đây
        foreach (Players player in listPlayer)
        {
            string regionName = listRegion.FirstOrDefault(r => r.ID == player.Region)?.Name ?? "Unknown";
            string rank = calculate_rank(player.Score);
            Debug.Log("ID: " + player.ID + "\nName: " + player.Name + "\nScore: " + player.Score + "\nRegion: " + regionName + "\nRank: " + rank);
        }
    }
    public void YC3()
    {
        ScoreKeeper scoreKeeper = ScoreKeeper.Instance;
        int currentScore = scoreKeeper.GetScore();

        var players = listPlayer
            .Where(player => player.Score < currentScore)
            .Select(player =>
            {
                string regionName = listRegion.FirstOrDefault(r => r.ID == player.Region)?.Name ?? "Unknown";
                string rank = calculate_rank(player.Score);
                return $"ID: {player.ID} | Name: {player.Name} | Score: {player.Score} | Region: {regionName} | Rank: {rank}";
            });

        string result = "YC3: Danh sách Player có Score bé hơn:\n" + string.Join("\n", players);

        Debug.Log(result);
    }
    
public void YC4(string text)
    {
        // sinh viên viết tiếp code ở đây

        if (int.TryParse(text, out int searchID))
        {
            Players foundPlayer = listPlayer.FirstOrDefault(p => p.ID == searchID);
            if (foundPlayer != null)
            {
                string regionName = listRegion.FirstOrDefault(r => r.ID == foundPlayer.Region)?.Name ?? "Unknown";
                string rank = calculate_rank(foundPlayer.Score);
                Debug.Log($"YC4: đã tìm thấy Player!\nID: {foundPlayer.ID}\nName: {foundPlayer.Name}\nScore: {foundPlayer.Score}\nRegion: {regionName}\nRank: {rank}");
            }
            else
            {
                Debug.Log("YC4: Không tìm thấy player với ID này.");
            }
        }
        else
        {
            Debug.Log("YC4: ID nhập vào không hợp lệ.");
        }
    }
    public void YC5()
    {
        // sinh viên viết tiếp code ở đây
        //Danh sách Player có Score theo thứ tự giảm dần
        var sortedPlayers = listPlayer.OrderByDescending(player => player.Score).ToList();
        foreach (var player in sortedPlayers)
        {
            string regionName = listRegion.FirstOrDefault(r => r.ID == player.Region)?.Name ?? "Unknown";
            string rank = calculate_rank(player.Score);
            Debug.Log($"YC5: Danh sách Player có Score theo thứ tự giảm dần:\nID: {player.ID}, Name: {player.Name}, Score: {player.Score}, Region: {regionName}, Rank: {rank}");
        }
    }
    public void YC6()
    {
        // sinh viên viết tiếp code ở đây
        // Xuất thông tin 5 player có score thấp nhất theo thứ tự tăng dần
        var lowestPlayers = listPlayer.OrderBy(player => player.Score).Take(5).ToList();
        foreach (var player in lowestPlayers)
        {
            string regionName = listRegion.FirstOrDefault(r => r.ID == player.Region)?.Name ?? "Unknown";
            string rank = calculate_rank(player.Score);
            Debug.Log($"YC6: 5 Player có Score thấp nhất:\nID: {player.ID}, Name: {player.Name}, Score: {player.Score}, Region: {regionName}, Rank: {rank}");
        }
    }
    public void YC7()
    {
        // sinh viên viết tiếp code ở đây
        Thread BXH = new Thread(() =>
        {
            SaveAllPlayersToRegionFile();
            CalculateAndSaveAverageScoreByRegion();

        });BXH.Name = "BXHRegion";
        BXH.Start();
        BXH.Join();
    }
    void CalculateAndSaveAverageScoreByRegion()
    {
        //Sinh viên viết tiếp code ở đây
        using (FileStream fileStream = new FileStream("bxhRegion.txt", FileMode.Create, FileAccess.Write))
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            foreach (var region in listRegion)
            {
                var playersInRegion = listPlayer.Where(player => player.Region == region.ID).ToList();
                if (playersInRegion.Count > 0)
                {
                    var averageScore = playersInRegion.Average(player => player.Score);
                    writer.WriteLine($"Region: {region.Name}, Average Score: {averageScore}");
                }
                else
                {
                    writer.WriteLine($"Region: {region.Name}, Average Score: 0 (No players)");
                }
            }
        }
    }
    void SaveAllPlayersToRegionFile()
    {
        using (FileStream fileStream = new FileStream("Player.txt", FileMode.Create, FileAccess.Write))
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            foreach (var player in listPlayer)
            {
                string regionName = listRegion.FirstOrDefault(r => r.ID == player.Region)?.Name ?? "Unknown";
                string rank = calculate_rank(player.Score);
                writer.WriteLine($"ID: {player.ID}, Name: {player.Name}, Score: {player.Score}, Region: {regionName}, Rank: {rank}");
            }
        }
    }

}

[SerializeField]
public class Region
{
    public int ID;
    public string Name;
    public Region(int ID, string Name)
    {
        this.ID = ID;
        this.Name = Name;
    }
}

[SerializeField]
public class Players
{
    // sinh viên viết tiếp code ở đây
    public int ID;
    public string Name;
    public int Score;
    public int Region;
    public Players(int ID, string Name, int Score, int Region)
    {
        this.ID = ID;
        this.Name = Name;
        this.Score = Score;
        this.Region = Region;
    }
}