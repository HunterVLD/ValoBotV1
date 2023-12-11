using MongoDB.Driver;

namespace ValoBotV1.BotComponents
{
    public static class InterfaceServices
    {
        public static async Task<string> RefactorTextAfterSearching(List<DataBase.MainTactics> items, string filterType)
        {
            string refactorResult = "";
            int indexOfList = 0;

            DataBase.Users creator;

            switch (filterType)
            {
                case "SearchByName":
                    creator = await items[0].Creator.ToEntityAsync();
                    string usersWhoCanReadStr = "Все";

                    if (items[0].TacticPolicy == "Private")
                    {
                        usersWhoCanReadStr = "";
                        var usersWhoCanRead = await items[0].UsersCanRead.ChildrenQueryable().ToListAsync();

                        foreach (var user in usersWhoCanRead)
                        {
                            usersWhoCanReadStr += user.UserName + " ";
                        }
                    }

                    refactorResult = $"Название 🖇: * {items[0].TacticName} *\n" +
                                     $"Создатель ✒: * {creator.UserName} *\n" +
                                     $"Доступ 🔐: * {items[0].TacticPolicy} *\n" +
                                     $"Имеют доступ 🔓: * {usersWhoCanReadStr} *\n" +
                                     $"Карта 🗞: * {items[0].Map} *\n" +
                                     $"Сторона 🗡🛡: * {items[0].AtackOrDefense} *\n\n" +
                                     $"---Тактика для сайта 📍*A:*-------------\n {items[0].ASite}\n" +
                                     "-----------------------------------------\n" +
                                     $"---Тактика для сайта 📍*B:*-------------\n {items[0].BSite}\n" +
                                     "-----------------------------------------\n" +
                                     $"---Тактика для сайта 📍*C:*-------------\n {items[0].CSite}\n" +
                                     "-----------------------------------------\n\n" +
                                     $"Описание/*Пик* 👩‍🦽: {items[0].DescriptionOpt}\n" +
                                     $"Возможное *фото* 🔘: {items[0].SomePhotoLinkOpt}\n";
                    
                    break;
                case "GetAll":
                    
                    foreach (var item in items.ToList())
                    {
                        creator = await items[indexOfList++].Creator.ToEntityAsync();
                                                
                        refactorResult +=
                            $"{indexOfList}) Создатель: *{creator.UserName}*  |  Карта: {item.Map}\n" +
                            $"     Сторона: {item.AtackOrDefense}\n" +
                            $"     Название 🖇: *{item.TacticName}*\n" +
                            "-----------------------------------------\n";
                    }
                    
                    break;
                case "GetMyTactics":
                    
                    foreach (var item in items.ToList())
                    {
                        creator = await items[indexOfList++].Creator.ToEntityAsync();
                        
                        refactorResult +=
                            $"{indexOfList}) Создатель: *{creator.UserName}*  |  Карта: {item.Map}\n" +
                            $"     Сторона: {item.AtackOrDefense}\n" +
                            $"     Название 🖇: *{item.TacticName}*\n" +
                            $"     Доступ: {item.TacticPolicy}\n" +
                            "--------------------------------------------\n";
                    }
                    
                    break;
                default:
                    return "Ошибка поиска";
            }

            return refactorResult;
        }

        public static async Task<string> RefactorRandomizingData(List<DataBase.MainTactics> items, int randIndex)
        {
            string refactorResult = "";

            DataBase.Users creator;
            
            creator = await items[randIndex].Creator.ToEntityAsync();
            string usersWhoCanReadStr = "Все";

            if (items[randIndex].TacticPolicy == "Private")
            {
                usersWhoCanReadStr = "";
                var usersWhoCanRead = await items[randIndex].UsersCanRead.ChildrenQueryable().ToListAsync();

                foreach (var user in usersWhoCanRead)
                {
                    usersWhoCanReadStr += user.UserName + " ";
                }
            }

            refactorResult = $"Название 🖇: * {items[randIndex].TacticName} *\n" +
                             $"Создатель ✒: * {creator.UserName} *\n" +
                             $"Доступ 🔐: * {items[randIndex].TacticPolicy} *\n" +
                             $"Имеют доступ 🔓: * {usersWhoCanReadStr} *\n" +
                             $"Карта 🗞: * {items[randIndex].Map} *\n" +
                             $"Сторона 🗡🛡: * {items[randIndex].AtackOrDefense} *\n\n" +
                             $"---Тактика для сайта 📍*A:*-------------\n {items[randIndex].ASite}\n" +
                             "-----------------------------------------\n" +
                             $"---Тактика для сайта 📍*B:*-------------\n {items[randIndex].BSite}\n" +
                             "-----------------------------------------\n" +
                             $"---Тактика для сайта 📍*C:*-------------\n {items[randIndex].CSite}\n" +
                             "-----------------------------------------\n\n" +
                             $"Описание/*Пик* 👩‍🦽: {items[randIndex].DescriptionOpt}\n" +
                             $"Возможное *фото* 🔘: {items[randIndex].SomePhotoLinkOpt}\n";


            return refactorResult;
        }
    }
}

