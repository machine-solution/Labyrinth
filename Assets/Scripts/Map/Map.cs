using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class Map : MonoBehaviour
{
    int iter = 0;
    GameObject[] sw;
    GameObject hospital_im;
    GameObject arsenal_im;
    Animator arsenal_anim;
    GameObject[] teleport_im;
    GameObject[] treasure_im;
    GameObject[] player_im;
    Animator[] player_anim;

    GameObject[] nick, face, juel;

    GameObject animeBut, backBut;
    Button animeStopBut, animePlayBut;
    Changer animeSpeedBut;
    Scroller scroller;

    public bool is_map = false;
    const float to_x = 0f, to_y = -4f;
    const float distS = 1.7f;
    const float distW = 0.85f;
    float mapScale;

    bool anime_pause = true;

    public void ShowPlayers()
    {
        nick = new GameObject[Base.numberOfPlayers];
        face = new GameObject[Base.numberOfPlayers];
        juel = new GameObject[Base.numberOfPlayers];
        for (int i = 0;i < Base.numberOfPlayers; ++i)
        {
            nick[i] = Instantiate(Resources.Load<GameObject>("Text"));
            nick[i].transform.position = new Vector3(-5.5f, 4f - i, 0);
            nick[i].GetComponent<TMPro.TextMeshPro>().text = Base.positions[0].players[i].name;

            face[i] = Instantiate(Resources.Load<GameObject>("human" ));
            face[i].GetComponent<SpriteRenderer>().color = Base.main.std_color[i];
            face[i].transform.position = new Vector3(-3f, 4f - i, 0);

            juel[i] = Instantiate(Resources.Load<GameObject>("treasure"));
            juel[i].transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = "0";
            juel[i].transform.position = new Vector3(-3.41f, 4.41f - i, 0);
        }
    }
    public void FixStats(int Phase)
    {
        for(int i = 0; i < Base.numberOfPlayers; ++i)
        {
            juel[i].transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = 
                Base.positions[Phase].players[i].treasures.ToString();
        }
    }

    public void ShowMap()
    {
        if (is_map)
            return;
//        HideResults();
        is_map = true;
        sw = new GameObject[3 * Base.size * Base.size + 4 * Base.size + 2];
        

        hospital_im = Instantiate(Resources.Load<GameObject>("hospital"));
        hospital_im.transform.position = new Vector3(to_x + mapScale * (distS + distS * Base.map.hospital.x),
                    to_y + mapScale * (distS + distS * Base.map.hospital.y), 0);
        hospital_im.transform.localScale = new Vector3(mapScale, mapScale, 1);

        arsenal_im = Instantiate(Resources.Load<GameObject>("arsenal"));
        arsenal_im.transform.position = new Vector3(to_x + mapScale * (distS + distS * Base.map.arsenal.x),
                    to_y + mapScale * (distS + distS * Base.map.arsenal.y), 0);
        arsenal_im.transform.localScale = new Vector3(mapScale, mapScale, 1);
        arsenal_anim = arsenal_im.GetComponent<Animator>();

        if (Base.teleportFlag)
        {
            teleport_im = new GameObject[2];
            teleport_im[0] = Instantiate(Resources.Load<GameObject>("teleport"));
            teleport_im[0].transform.position = new Vector3(to_x + mapScale * (distS + distS * Base.map.teleport[0].x),
                        to_y + mapScale * (distS + distS * Base.map.teleport[0].y), 0);
            teleport_im[0].transform.localScale = new Vector3(mapScale, mapScale, 1);

            teleport_im[1] = Instantiate(Resources.Load<GameObject>("teleport"));
            teleport_im[1].transform.position = new Vector3(to_x + mapScale * (distS + distS * Base.map.teleport[1].x),
                        to_y + mapScale * (distS + distS * Base.map.teleport[1].y), 0);
            teleport_im[1].transform.localScale = new Vector3(mapScale, mapScale, 1);
        }

        for (int i = 0; i < Base.size; ++i)
        {
            for (int j = 0; j < Base.size; ++j)
            {
                sw[iter] = Instantiate(Resources.Load<GameObject>("square"));
                sw[iter].transform.position = new Vector3(to_x + mapScale * (distS + distS * i),
                    to_y + mapScale * (distS + distS * j), 0);
                sw[iter].transform.localScale = new Vector3(mapScale, mapScale, 1);
                ++iter;
            }
        }
        for (int i = 0; i < Base.size; ++i)
        {
            for (int j = 0; j <= Base.size; ++j)
            {
                if (Base.map.walls[0, i, j].getHave())
                {
                    sw[iter] = Instantiate(Resources.Load<GameObject>("wall0"));
                    sw[iter].transform.position = new Vector3(
                        to_x + mapScale * (distS / 2 + distS * i + distW),
                        to_y + mapScale * (distS / 2 + distS * j),
                        0);
                    sw[iter].transform.localScale = new Vector3(mapScale, mapScale, 1);
                    ++iter;
                }
            }
        }
        for (int i = 0; i <= Base.size; ++i)
        {
            for (int j = 0; j < Base.size; ++j)
            {
                if (Base.map.walls[1, i, j].getHave())
                {
                    sw[iter] = Instantiate(Resources.Load<GameObject>("wall1"));
                    sw[iter].transform.position = new Vector3(
                        to_x + mapScale * (distS / 2 + distS * i),
                        to_y + mapScale * (distS / 2 + distS * j + distW),
                        0);
                    sw[iter].transform.localScale = new Vector3(mapScale, mapScale, 1);
                    ++iter;
                }
            }
        }

        Paint_start();
        Fix_players(0);
        Paint_treasures(0);
        anime_pause = true;
        speed = 1;
        animeBut.SetActive(true);
        if (anime_pause)
        {
            animePlayBut.Unlock();
            animeStopBut.Block();
        }
        else
        {
            animePlayBut.Block();
            animeStopBut.Unlock();
        }
        StartCoroutine("Anime");
    }

    public void Paint_start()
    {
        //_____________________________________
        player_im = new GameObject[Base.numberOfPlayers];
        player_anim = new Animator[Base.numberOfPlayers];
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            float delta_x = mapScale * System.Convert.ToSingle(distS * 0.2 * Cos(2 * i * PI / Base.numberOfPlayers));
            float delta_y = mapScale * System.Convert.ToSingle(distS * 0.2 * Sin(2 * i * PI / Base.numberOfPlayers));
            player_im[i] = Instantiate(Resources.Load<GameObject>("player"));
            player_anim[i] = player_im[i].GetComponent<Animator>();
            player_im[i].transform.GetChild(0).position = new Vector3(delta_x, delta_y, 0);
            player_im[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Base.main.std_color[i];
            player_im[i].transform.GetChild(1).position = new Vector3(delta_x, delta_y, 0);
            player_im[i].transform.GetChild(2).position = new Vector3(delta_x, delta_y, 0);
            player_im[i].transform.position = new Vector3(
                to_x + mapScale * (distS + distS * Base.positions[0].players[i].location.x),
                to_y + mapScale * (distS + distS * Base.positions[0].players[i].location.y),
                0);
            player_im[i].transform.localScale = new Vector3(mapScale, mapScale, 1);
        }
    }
    public void Fix_players(int Phase)
    {
        for (int i = 0; i < Base.numberOfPlayers; ++i)
        {
            player_im[i].transform.position = new Vector3(
                to_x + mapScale * (distS + distS * Base.positions[Phase].players[i].location.x),
                to_y + mapScale * (distS + distS * Base.positions[Phase].players[i].location.y),
                0);
            player_im[i].transform.localScale = new Vector3(mapScale, mapScale, 1);
        }
    }
    public void Paint_treasures(int Phase)
    {
        
        int[,] cnt = new int[Base.size, Base.size];
        for (int x = 0; x < Base.size; ++x)
            for (int y = 0; y < Base.size; ++y)
                cnt[x, y] = 0;
        int nos = 0;
        for (int i = 0; i < Base.numberOfTreasures; ++i)
        {
            if (Base.positions[Phase].trNum[i] == Base.numberOfPlayers)
            {
                if (cnt[Base.positions[Phase].treasure[i].x, Base.positions[Phase].treasure[i].y] == 0)
                {
                    ++nos;
                }
                cnt[Base.positions[Phase].treasure[i].x, Base.positions[Phase].treasure[i].y] += 1;
            }
        }

        treasure_im = new GameObject[nos];
        int no = 0;
        for (int x = 0; x < Base.size; ++x)
            for (int y = 0; y < Base.size; ++y)
            {
                if (cnt[x, y] > 0)
                {
                    treasure_im[no] = Instantiate(Resources.Load<GameObject>("treasure"));
                    treasure_im[no].transform.position = new Vector3(
                    to_x + mapScale * (distS + distS * x),
                    to_y + mapScale * (distS + distS * y),
                    0);
                    treasure_im[no].transform.localScale = new Vector3(mapScale, mapScale, 1);
                    treasure_im[no].transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = cnt[x, y].ToString();
                    ++no;
                }
            }
    }
    public void Erase_treasures()
    {
        for (int i = 0; i < treasure_im.Length; ++i)
            Destroy(treasure_im[i]);
        treasure_im = new GameObject[0];
    }

    public void Fix(int Phase)
    {

        Fix_players(Phase);
        Erase_treasures();
        Paint_treasures(Phase);
        scroller.SetVal(Phase);
        FixStats(Phase);
    }

    const int cntIt = 100;
    const int speedKnife = 1;
    const int speedBullet = 2;
    const int speedCracker = 1;
    int speed = 1;
    const float startMoveTime = 0.01f;
    float moveTime = 0.01f;

    public Vector3 GetVect(string side)
    {
        if (side == "up")
            return new Vector3(0, 1, 0);
        if (side == "right")
            return new Vector3(1, 0, 0);
        if (side == "down")
            return new Vector3(0, -1, 0);
        if (side == "left")
            return new Vector3(-1, 0, 0);
        return new Vector3(0, 0, 0);

    }
    public Vector3 GetAngle(string side)
    {
        if (side == "up")
            return new Vector3(0, 0, 90);
        if (side == "right")
            return new Vector3(0, 0, 0);
        if (side == "down")
            return new Vector3(0, 0, 270);
        if (side == "left")
            return new Vector3(0, 0, 180);
        return new Vector3(0, 0, 0);
    }
    public void StopAnime()
    {
        if (!anime_pause)
            anime_pause = true;
        if (anime_pause)
        {
            animePlayBut.Unlock();
            animeStopBut.Block();
        }
        else
        {
            animePlayBut.Block();
            animeStopBut.Unlock();
        }
    }
    public void PlayAnime()
    {
        if (anime_pause)
            anime_pause = false;
        if (anime_pause)
        {
            animePlayBut.Unlock();
            animeStopBut.Block();
        }
        else
        {
            animePlayBut.Block();
            animeStopBut.Unlock();
        }
    }
    public void ChangeSpeed(int Speed)
    {
        if (speed != Speed)
        {
            speed = Speed;
            moveTime = startMoveTime / speed;
            for (int i = 0; i < Base.numberOfPlayers; ++i)
            {
                player_anim[i].SetFloat("speed", speed);
                player_anim[i].transform.GetChild(1).GetComponent<Animator>().SetFloat("speed", speed);
                player_anim[i].transform.GetChild(2).GetComponent<Animator>().SetFloat("speed", speed);
            }
            arsenal_anim.SetFloat("speed", speed);
        }
    }
    public void Rewind(int Phase)
    {
        StopCoroutine("Anime");
        phase = Phase;
        StartCoroutine("Anime");
        StopAnime();
        Fix(phase);
    }

    public bool HavePlayers(Coord cor)
    {
        for (int i = 0; i < Base.positions[phase].numberOfPlayers; ++i)
            if (cor == Base.positions[phase].players[i].location)
                return true;
        return false;
    }

    // this block using Base.positions[phase].index && Base.actionSide &&
    int phase = 0;
    public IEnumerator StepWall()
    {
        player_anim[Base.positions[phase].index].SetInteger("state", 1);
        Vector3 side = GetVect(Base.transitions[phase].side)
            * distS * mapScale / cntIt; //cntIt = 100;
        int cnt = 0;
        int lim = 30; // 30/100 * distS * mapScale
        float timer = 0f;
        while (cnt < lim)
        {
            timer += Time.deltaTime;
            int count = 0;
            while (timer >= moveTime && cnt + count < lim)
            {
                ++count;
                timer -= moveTime;
            }
            cnt += count;
            player_im[Base.positions[phase].index].transform.position += side * count;

            yield return null;
        }
        cnt = 0;
        timer = 0f;
        while (cnt < lim)
        {
            timer += Time.deltaTime;
            int count = 0;
            while (timer >= moveTime && cnt + count < lim)
            {
                ++count;
                timer -= moveTime;
            }

            cnt += count;
            player_im[Base.positions[phase].index].transform.position -= side * count;

            yield return null;
        }

        player_anim[Base.positions[phase].index].SetInteger("state", 0);
    }
    public IEnumerator StepGo()
    {
        player_anim[Base.positions[phase].index].SetInteger("state", 1);
        Vector3 side = GetVect(Base.transitions[phase].side)
            * distS * mapScale / cntIt; //cntIt = 100;
        int cnt = 0;
        int lim = 100; // 100/100 * distS * mapScale
        float timer = 0f;
        while (cnt < lim)
        {
            timer += Time.deltaTime;
            int count = 0;
            while (timer >= moveTime && cnt + count < lim)
            {
                ++count;
                timer -= moveTime;
            }
            cnt += count;
            player_im[Base.positions[phase].index].transform.position += side * count;

            yield return null;
        }

        player_anim[Base.positions[phase].index].SetInteger("state", 0);
    }
    public IEnumerator StrikeWall()
    {
        GameObject knife = Instantiate(Resources.Load<GameObject>("knife"));
        knife.transform.position = new Vector3(
            to_x + mapScale * (distS + distS * Base.positions[phase].players[Base.positions[phase].index].location.x),
            to_y + mapScale * (distS + distS * Base.positions[phase].players[Base.positions[phase].index].location.y),
            0);
        knife.transform.Rotate(GetAngle(Base.transitions[phase].side));
        knife.transform.localScale *= mapScale;

        Vector3 side = GetVect(Base.transitions[phase].side)
            * distS * mapScale / cntIt; //cntIt = 100;
        int cnt = 0;
        int lim = 30; // 30/100 * distS * mapScale
        float timer = 0f;
        while (cnt < lim)
        {
            timer += Time.deltaTime;
            int count = 0;
            while (timer >= moveTime && cnt + count < lim)
            {
                ++count;
                timer -= moveTime;
            }
            cnt += count;
            knife.transform.position += side * count;

            yield return null;
        }
        Destroy(knife);
    }
    public IEnumerator StrikeGo()
    {
        GameObject knife = Instantiate(Resources.Load<GameObject>("knife"));
        knife.transform.position = new Vector3(
            to_x + mapScale * (distS + distS * Base.positions[phase].players[Base.positions[phase].index].location.x),
            to_y + mapScale * (distS + distS * Base.positions[phase].players[Base.positions[phase].index].location.y),
            0);
        knife.transform.Rotate(GetAngle(Base.transitions[phase].side));
        knife.transform.localScale *= mapScale;

        Vector3 side = GetVect(Base.transitions[phase].side)
            * distS * mapScale / cntIt; //cntIt = 100;
        int cnt = 0;
        int lim = 80; // 80/100 * distS * mapScale
        float timer = 0f;
        while (cnt < lim)
        {
            timer += Time.deltaTime;
            int count = 0;
            while (timer >= moveTime && cnt + count < lim)
            {
                ++count;
                timer -= moveTime;
            }
            cnt += count;
            knife.transform.position += side * count;

            yield return null;
        }
        Destroy(knife);
    }
    public IEnumerator FireWall(GameObject bullet)
    {
        Vector3 side = GetVect(Base.transitions[phase].side)
            * distS * mapScale / cntIt; //cntIt = 100; 
        int cnt = 0;
        int lim = 30; // 30/100 * distS * mapScale
        float timer = 0f;
        while (cnt < lim)
        {
            timer += Time.deltaTime;
            int count = 0;
            while (timer >= moveTime / speedBullet && cnt + count < lim)
            {
                ++count;
                timer -= moveTime / speedBullet;
            }
            cnt += count;
            bullet.transform.position += side * count;

            yield return null;
        }
    }
    public IEnumerator FireGo(GameObject bullet)
    {
        Vector3 side = GetVect(Base.transitions[phase].side)
            * distS * mapScale / cntIt; //cntIt = 100;
        int cnt = 0;
        int lim = 100; // 100/100 * distS * mapScale
        float timer = 0f;
        while (cnt < lim)
        {
            timer += Time.deltaTime;
            int count = 0;
            while (timer >= moveTime / speedBullet && cnt + count < lim)
            {
                ++count;
                timer -= moveTime / speedBullet;
            }
            cnt += count;
            bullet.transform.position += side * count;

            yield return null;
        }
    }
    public IEnumerator Fire()
    {
        GameObject bullet = Instantiate(Resources.Load<GameObject>("bullet"));
        bullet.transform.position = new Vector3(
            to_x + mapScale * (distS + distS * Base.positions[phase].players[Base.positions[phase].index].location.x),
            to_y + mapScale * (distS + distS * Base.positions[phase].players[Base.positions[phase].index].location.y),
            0);
        bullet.transform.Rotate(GetAngle(Base.transitions[phase].side));
        bullet.transform.localScale *= mapScale;

        Coord bul = Base.positions[phase].players[Base.positions[phase].index].location;
        Coord cor = bul;
        while (true)
        {
            if (bul == cor)
                cor = Base.Move(cor, Base.transitions[phase].side);

            if (cor == bul || cor.x == -1 || cor.x == Base.size ||
                cor.y == -1 || cor.y == Base.size)
            {
                yield return StartCoroutine("FireWall", bullet);
                break;
            }
            else
            {
                yield return StartCoroutine("FireGo", bullet);
                if (HavePlayers(cor))
                {
                    Destroy(bullet);
                    yield return StartCoroutine("KillSquare", cor);
                    break;
                }

                bul = cor;
                cor = Base.Teleporting(cor);
                bullet.transform.position = new Vector3(
                    to_x + mapScale * (distS + distS * cor.x),
                    to_y + mapScale * (distS + distS * cor.y),
                    0);
            }
        }
        if (bullet)
            Destroy(bullet);
    }
    public IEnumerator ThrowGo(GameObject cracker)
    {
        Vector3 side = GetVect(Base.transitions[phase].side)
            * distS * mapScale / cntIt; //cntIt = 100;
        int cnt = 0;
        int lim = 100; // 100/100 * distS * mapScale
        float timer = 0f;
        while (cnt < lim)
        {
            timer += Time.deltaTime;
            int count = 0;
            while (timer >= moveTime && cnt + count < lim)
            {
                ++count;
                timer -= moveTime;
            }
            cnt += count;
            cracker.transform.position += side * count;

            yield return null;
        }
    }
    public IEnumerator Throw()
    {
        GameObject cracker = Instantiate(Resources.Load<GameObject>("cracker"));
        cracker.transform.position = new Vector3(
            to_x + mapScale * (distS + distS * Base.positions[phase].players[Base.positions[phase].index].location.x),
            to_y + mapScale * (distS + distS * Base.positions[phase].players[Base.positions[phase].index].location.y),
            0);
        cracker.transform.localScale *= mapScale;
        cracker.GetComponent<Animator>().SetFloat("speed", speed);

        Coord cra = Base.positions[phase].players[Base.positions[phase].index].location;
        Coord cor = cra;
        int dist = 2;
        while (dist-- > 0)
        {
            if (cra == cor)
                cor = Base.Move(cor, Base.transitions[phase].side);
            else
                dist++;

            if (cor == cra || cor.x == -1 || cor.x == Base.size ||
                cor.y == -1 || cor.y == Base.size)
            {
                yield return StartCoroutine("ExplodeCracker", cracker);
                break;
            }
            else
            {
                yield return StartCoroutine("ThrowGo", cracker);
                if (HavePlayers(cor))
                {
                    yield return StartCoroutine("ExplodeCracker", cracker);
                    break;
                }

                cra = cor;
                cor = Base.Teleporting(cor); 
                cracker.transform.position = new Vector3(
                     to_x + mapScale * (distS + distS * cor.x),
                     to_y + mapScale * (distS + distS * cor.y),
                     0);
            }
        }
        if (dist < 0)
            yield return StartCoroutine("ExplodeCracker", cracker);
        Destroy(cracker);
    }

    static bool endOfClip = false;
    public static void EndAnimeClip()
    {
        endOfClip = true;
    }
    
    public IEnumerator GetKnife()
    {
        arsenal_anim.SetInteger("state", 0);
        while (!endOfClip)
        {
            yield return null;
        }
        endOfClip = false;
        arsenal_anim.SetInteger("state", -1);
    }
    public IEnumerator GetBullet()
    {
        arsenal_anim.SetInteger("state", 1);
        while (!endOfClip)
        {
            yield return null;
        }
        endOfClip = false;
        arsenal_anim.SetInteger("state", -1);
    }
    public IEnumerator GetArmor()
    {
        arsenal_anim.SetInteger("state", 2);
        while (!endOfClip)
        {
            yield return null;
        }
        endOfClip = false;
        arsenal_anim.SetInteger("state", -1);
    }
    public IEnumerator GetCracker()
    {
        arsenal_anim.SetInteger("state", 3);
        while (!endOfClip)
        {
            yield return null;
        }
        endOfClip = false;
        arsenal_anim.SetInteger("state", -1);
    }
    public IEnumerator ExplodeCracker(GameObject cracker)
    {
        cracker.GetComponent<Animator>().SetBool("explode", true);
        while (!endOfClip)
        {
            yield return null;
        }
        endOfClip = false;
        cracker.GetComponent<Animator>().SetBool("explode", false);
    }
    public IEnumerator KillSquare(Coord cor)
    {
        endOfClip = true;
        for (int i = 0; i< Base.numberOfPlayers; ++i)
        {
            if (Base.positions[phase].players[i].location == cor)
            {
                if (Base.positions[phase].players[i].armors > 0)
                {
                    endOfClip = false;
                    player_im[i].transform.GetChild(1).GetComponent<Animator>().SetTrigger("defend");
                }
                else
                {
                    endOfClip = false;
                    player_im[i].transform.position = new Vector3(
                        to_x + mapScale * (distS + distS * Base.map.hospital.x),
                        to_y + mapScale * (distS + distS * Base.map.hospital.y),
                        0);
                    player_im[i].transform.GetChild(2).GetComponent<Animator>().SetTrigger("rebirth");
                }
            }
        }
        Erase_treasures();
        Paint_treasures(phase + 1);
        while (!endOfClip)
        {
            yield return null;
        }
        endOfClip = false;
    }

    public IEnumerator Anime()
    {
        while (phase < Base.transitions.Size())
        {

            while (anime_pause)
            {
                yield return null;
            }

            if (Base.transitions[phase].type == "step")
            {
                if (Base.transitions[phase].answer == "wall\n" || Base.transitions[phase].answer == "exit\n")
                {
                    yield return StartCoroutine("StepWall");
                }
                else
                {
                    yield return StartCoroutine("StepGo");
                    Fix(phase + 1);
                    if (Base.transitions[phase].arsenal)
                    {
                        switch (Base.positions[phase].arsSupply)
                        {
                            case 0:
                                yield return StartCoroutine("GetKnife");
                                break;
                            case 1:
                                yield return StartCoroutine("GetBullet");
                                break;
                            case 2:
                                yield return StartCoroutine("GetArmor");
                                break;
                            case 3:
                                yield return StartCoroutine("GetCracker");
                                break;
                        }
                    }
                }
            }
            if (Base.transitions[phase].type == "strike")
            {
                if (Base.transitions[phase].answer == "wall\n")
                {
                    yield return StartCoroutine("StrikeWall");
                }
                else
                {
                    yield return StartCoroutine("StrikeGo");
                    yield return StartCoroutine("KillSquare", 
                        Base.Move(Base.positions[phase].players[Base.positions[phase].index].location,
                        Base.transitions[phase].side));
                }
            }
            if (Base.transitions[phase].type == "fire")
            {
                yield return StartCoroutine("Fire");
            }
            if (Base.transitions[phase].type == "throw")
            {
                yield return StartCoroutine("Throw");
            }

            Fix(phase + 1);
            ChangeSpeed(animeSpeedBut.val);
            ++phase;
            yield return new WaitForSeconds(.5f / speed);
        }
        StopAnime();
    }

    [System.Obsolete]
    void Start()
    {
        mapScale = (8f / Base.size) / (1f * distS);
        if (mapScale > 1)
            mapScale = 1f;
        animeBut = GameObject.Find("AnimeBut");
        backBut = GameObject.Find("BackBut");
        if (Base.is_tournament)
            backBut.GetComponent<Button>().click = Base.main.OnScene_TourEnd;
        else
            backBut.GetComponent<Button>().click = Base.main.OnScene_End; 
        animeStopBut = animeBut.transform.GetChild(1).GetComponent<Button>();
        animePlayBut = animeBut.transform.GetChild(0).GetComponent<Button>();
        animeSpeedBut = animeBut.transform.GetChild(2).GetComponent<Changer>();
        scroller = animeBut.transform.GetChild(3).GetComponent<Scroller>();
        animeStopBut.click = StopAnime;
        animePlayBut.click = PlayAnime;
        scroller.ResetLimits(0, Base.transitions.Size());
        scroller.Drag = Rewind;
        animeBut.SetActive(false);

        phase = 0;
        ShowMap();
        ShowPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSpeed(animeSpeedBut.val);
    }
}
