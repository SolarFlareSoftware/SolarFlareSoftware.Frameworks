/*
 * Copyright (C) 2023 Solar Flare Software, Inc.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
 * PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.[8]
 * 
 */
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
