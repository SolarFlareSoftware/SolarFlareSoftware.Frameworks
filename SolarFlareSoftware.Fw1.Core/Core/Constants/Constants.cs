namespace SolarFlareSoftware.Fw1.Core
{
    public partial class Constants
    {
        //public const string ACTION_NOTIFICATION_TEMP_DATA = "ActionNotificationViewModel";
        public const string AUDIT_COLUMNS_CREATED_DATE = "AuditAddDate";
        public const string AUDIT_COLUMNS_CREATED_BY_USER = "AuditAddUser";
        public const string AUDIT_COLUMNS_UPDATED_DATE = "AuditUpdateDate";
        public const string AUDIT_COLUMNS_UPDATED_BY_USER = "AuditUpdateUser";
        public const string GENERIC_SYSTEM_USER_NAME = "System";

        #region BROADCAST CONSTANTS
        public const int BROADCAST_MESSAGE_TYPE_NORMAL = 1;
        public const int BROADCAST_MESSAGE_TYPE_CRITICAL = 2;
        public const string BROADCAST_MESSAGE_TYPE_NORMAL_CLASS = "message-type-normal";
        public const string BROADCAST_MESSAGE_TYPE_CRITICAL_CLASS = "message-type-critical";
        public const int BROADCAST_MESSAGE_MODE_BANNER = 1;
        public const int BROADCAST_MESSAGE_MODE_WIDGET = 2;
        #endregion

        #region HISTORY ACTION CODE
        public const int HISTORY_TABLE_ACTION_INSERT = 1;
        public const int HISTORY_TABLE_ACTION_UPDATE = 2;
        public const int HISTORY_TABLE_ACTION_DELETE = 3;
        #endregion

        // Database Action Type
        public const int DATABASE_ACTION_ADD = 1;
        public const int DATABASE_ACTION_GET = 2;
        public const int DATABASE_ACTION_UPDATE = 3;
        public const int DATABASE_ACTION_DELETE = 4;

    }
}
