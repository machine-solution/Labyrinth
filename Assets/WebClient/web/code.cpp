bool online ... /*псевдокод-идея, как можно попробовать это реализовывать*/ 
online = true ...
/*сейчас ход i-ого игрока*/
if (index<=i && i<index+k) //k - число игроков на данном устройстве; если online == false, то index=0, k=players
{
    /*стандартный игровой процесс*/
    step =...
    side =...
    /*доходим до получения результата его хода*/
    gameans =...
    if (online)
    {
        string d = delimeter, str = step + d + side + d + gameans;
        Web.set(str); 
        bool cycle = true;
        while (cycle)
        {
            /*меняем кадры*/
            if (Web.res == "");
            else if (Web.res == "-1")
            {
                /*проверьте подключение к интернету*/
                Web.set(str);
            } 
            else break; 
        }
    } ...
}
else 
{
    /*заметь, в эту часть кода мы можем попасть, только при online==true*/
    /*скроем все клавиши выбора хода от пользователя*/
    Web.get();
    bool cycle = true;
    while (cycle)
    {
        /*меняем кадры*/
        if (Web.res == "");
        else if (Web.res == "-1")
        {
            /*проверьте подключение к интернету*/
            Web.set(str);
        } 
        else if (Web.res == "1") Web.get();
        else break; 
    }
    Parse(str, out step, out side, out gameans);
    /*обрабатываем step, side как будто их пользователь и ввёл
    (получается от gameans можно и отказаться)*/ ...
} ...


