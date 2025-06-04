using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Threading;
using System;

public class ASM_MN : Singleton<ASM_MN>
{
    public List<Region> listRegion = new List<Region>();
    public List<Players> listPlayer = new List<Players>();

    private void Start()
    {
        createRegion();
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
        // sinh viên viết tiếp code ở đây
        //Thong tin cac player co score be hon score hien tai cua nguoi choi

    }
    public void YC4()
    {
        // sinh viên viết tiếp code ở đây
    }
    public void YC5()
    {
        // sinh viên viết tiếp code ở đây
    }
    public void YC6()
    {
        // sinh viên viết tiếp code ở đây
    }
    public void YC7()
    {
        // sinh viên viết tiếp code ở đây
    }
    void CalculateAndSaveAverageScoreByRegion()
    {
        // sinh viên viết tiếp code ở đây
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