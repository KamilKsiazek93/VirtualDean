using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDean.Enties
{
    public static class ActionResultMessage
    {
        public const string UnauthorizedUser = "Nie masz uprawnień do przeprowadzenia tej operacji";

        /**
         * Brothers messages
         */
        public const string BrotherAlreadyExist = "W bazie istnieje już brat o takim imieniu i nazwisku";
        public const string BrotherNotFound = "Nie ma takiego brata w bazie danych";
        public const string BrotherDeleted = "Brat został usunięty";

        /**
         * Office messages
         */

        public const string OfficeAdded = "Dodano oficja";
        public const string OfficeNotAdded = "Nie udało się dodać oficjów";
        public const string DataUpdated = "Zaktualizowano dane";
        public const string OperationFailed = "Operacja się nie powiodła";

        /**
         * Obstacle messages
         */

        public const string ObstacleAdded = "Dodano przeszkodę";
        public const string ObstacleNotFound = "Nie ma takiej przeszkody";
        public const string ObstacleNotAdded = "Nie udało się dodać przeszkody";
        public const string ObstacleDeleted = "Przeszkoda została usunięta";
    }
}
