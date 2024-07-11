namespace EmployeeMgtApi
{
    public static class Constants
    {
        public static class Messages
        {
            // Success Messages
            public const string Success = "Success";
            public const string EmployeeCreated = "Employee created successfully";
            public const string EmployeeUpdated = "Employee updated successfully";
            public const string EmployeeDeleted = "Employee deleted successfully";

            // Error Messages
            public const string EmployeeNotFound = "Employee not found";
            public const string InvalidData = "Invalid data";
            public const string InternalServerError = "Internal Server Error";

            // Message indicating an employee was marked as inactive
            public const string EmployeeInactive = "Employee marked as inactive.";
        }
    }
}
