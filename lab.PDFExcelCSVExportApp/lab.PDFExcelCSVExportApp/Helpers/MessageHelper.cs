namespace lab.PDFExcelCSVExportApp.Helpers
{
    public static class MessageHelper
    {
        public static string MessageTypeInfo="info";
        public static string MessageTypeWarning="warning";
        public static string MessageTypeSuccess="success";
        public static string MessageTypeDanger="danger";

        public static string Save="Saved Successfully.";
        public static string Update="Updated Successfully.";
        public static string Delete="Deleted Successfully.";
        public static string Add="Added Successfully.";
        public static string Edit="Edited Successfully.";
        public static string Remove="Removed Successfully.";

        public static string SaveFail="Couldn't Saved Successfully.";
        public static string UpdateFail="Couldn't Updated Successfully.";
        public static string DeleteFail="Couldn't Deleted Successfully.";
        public static string AddFail="Couldn't Added Successfully.";
        public static string EditFail="Couldn't Edited Successfully.";
        public static string RemoveFail="Couldn't Removed Successfully.";

        public static string FilesUploaded = "'{0}' Files Uploaded Successfully.";
        public static string FilesUploadedFail = "Files Couldn't Uploaded Successfully.";
        public static string FilesNoSelected = "No Files Selected.";
        public static string FilesAlreadyExists = "Already Exists. Files Couldn't Uploaded Successfully.";
        public static string NameAlreadyExists = "Name already taken. Please choose another name.";
        public static string UrlAlreadyExists = "Url already taken. Please choose another url.";
        public static string AlreadyExists = "Already Exists.";
        public static string AlreadyAdded = "Already added one, So you can edit that one.";
        public static string Success="Successfully.";
        public static string Info="Please contact your system admin.";
        public static string Warning="Please contact your system admin.";
        public static string Error="We are facing some problem while processing the current request. Please try again later.";
        public static string UnhandledError="We are facing some problem while processing the current request. Please try again later.";
        public static string UnAuthenticated="You are not authenticated user.";
        public static string NullError="Requested object could not found.";
        public static string NullReferenceExceptionError="There are one or more required fields that are missing.";

        public static string IsEmailExists = "Email '{0}' already taken. Please choose another email.";
        public static string IsEmailNotExists = "Email '{0}' is not taken.";

        public static string SentMessage = "Message Sent Successfully.";
        public static string SentMessageFail = "Couldn't Message Sent Successfully.";
        public static string CaptchaSecurityCode = "Please enter the security code as a number.";

        public static string DataNotFound = "Data not found.";
        public static string CreateDbAndInsertMasterData = "Created database and inserted master data.";
        public static string CreateDbAndInsertMasterDataFail = "Couldn't created database and inserted master data.";
        public static string AlreadyDbExists = "Already Database Exists.";

        public static string InvalidDateTime = "The {0} is not recognized as a valid Date.";
        public static string InvalidBeginEndDateTime = "The {0} shouldn't be greater than {1}.";
        public static string InvalidEndDateTime = "The {0} should be provided.";

        public static string FileImported = "File uploaded successfully!";
        public static string FileParseFail = "Problem parsing file!";
        public static string FilesImportedFail = "File upload failed!";
        public static string DuplicateCustomField = "Couldn't be added duplicate custom field!";

        public static string DuplicateInsertion = "Data row is not exists!";
        public static string DuplicateInsertionFail = "Insertion failure. Data row already exists!";

        public static string InternalServerError = "Internal Server error.";
        public static string LoginUserDeleteFail = "Delete failed. You couldn't deleted login user!";
        public static string AdminRoleUserDeleteFail = "Delete failed. You couldn't deleted admin role user!";

        public static string UserExists = "Email is already taken. Please choose another email!";
        public static string UserNotExists = "Email is available!";

        public static string BarCodeScanner = "Bar code scanner successfully!";
        public static string BarCodeScannerFail = "Bar code scanner failed!";
    }
}
