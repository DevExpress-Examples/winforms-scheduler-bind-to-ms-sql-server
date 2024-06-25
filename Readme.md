<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128633804/18.1.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E551)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# WinForms Scheduler - Bind to MS SQL Server (runtime)

This example shows how to bind the `SchedulerControl` to a MS SQL Server database at runtime. The `SqlCommandBuilder` class is used to generate SQL queries.

The example uses the MS SQL Server database. You can create a new database using the *.sql* script file included in the project, or connect to another database. Check the code that specifies mappings to ensure that all data fields are correctly mapped to appointment properties.

In this example:

* Define mappings.
  
  The `UniqueID` field in the **Appointments** table is not mapped because it is an identity auto-incremented field updated by MS SQL Sever. If you map it, make sure that the [AppointmentStorage.CommitIdToDataSource](https://docs.devexpress.com/WindowsForms/DevExpress.XtraScheduler.AppointmentStorage.CommitIdToDataSource) option is disabled.
  
  ```csharp
  private void MapAppointmentData() {
      this.schedulerDataStorage1.Appointments.Mappings.AllDay = "AllDay";
      this.schedulerDataStorage1.Appointments.Mappings.Description = "Description";
      // Required mapping.
      this.schedulerDataStorage1.Appointments.Mappings.End = "EndDate";
      this.schedulerDataStorage1.Appointments.Mappings.Label = "Label";
      this.schedulerDataStorage1.Appointments.Mappings.Location = "Location";
      this.schedulerDataStorage1.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo";
      this.schedulerDataStorage1.Appointments.Mappings.ReminderInfo = "ReminderInfo";
      // Required mapping.
      this.schedulerDataStorage1.Appointments.Mappings.Start = "StartDate";
      this.schedulerDataStorage1.Appointments.Mappings.Status = "Status";
      this.schedulerDataStorage1.Appointments.Mappings.Subject = "Subject";
      this.schedulerDataStorage1.Appointments.Mappings.Type = "Type";
      this.schedulerDataStorage1.Appointments.Mappings.ResourceId = "ResourceIDs";
      this.schedulerDataStorage1.Appointments.CustomFieldMappings.Add(new AppointmentCustomFieldMapping("MyNote", "CustomField1"));
  }
  ```
* Set the `ResourceSharing` property to **true** to assign multiple resources to appointments.
  
  ```csharp
  this.schedulerDataStorage1.Appointments.ResourceSharing = true;
  ```
  
  If the `ResourceSharing` property is set to **false** (the default value), the [AppointmentMappingInfo.ResourceId](https://docs.devexpress.com/CoreLibraries/DevExpress.XtraScheduler.AppointmentMappingInfo.ResourceId) property should be set to the database field that contains the value of the resource ID that is associated with the appointment. This field must contain values of the same type as the resource ID. The `Scheduler` control does not restrict the type of resource ID to a particular .NET type. You can use any data type if the types of corresponding fields in **Appointment** and **Resource** data tables match.

If your database server is not MS SQL, you can replace `SqlDataAdapter` and `SqlCommandBuilder` with another data adapter and command builder (for example, `OracleDataAdapter` and `OracleCommandBuilder`).


## Documentation

* [Data Binding - WinForms Scheduler](https://docs.devexpress.com/WindowsForms/8386/controls-and-libraries/scheduler/data-binding)
* [Mappings](https://docs.devexpress.com/WindowsForms/15468/controls-and-libraries/scheduler/data-binding/mappings)
* [Create a Sample SQL Database for Scheduler Appointments and Resources](https://docs.devexpress.com/WindowsForms/9605/controls-and-libraries/scheduler/data-binding/data-sources/microsoft-sql-server)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=winforms-scheduler-bind-to-ms-sql-server&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=winforms-scheduler-bind-to-ms-sql-server&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
