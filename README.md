# EduPlatform {{Project Under Construction}}


[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2019+-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![Stripe](https://img.shields.io/badge/Stripe-626CD9?logo=stripe&logoColor=white)](https://stripe.com)
[![Entity Framework](https://img.shields.io/badge/Entity_Framework_Core-8.0-6DB33F?logo=ef&logoColor=white)](https://docs.microsoft.com/ef/)
[![Postman](https://img.shields.io/badge/Postman-API-orange?logo=postman)](https://www.postman.com/)
[![Swagger](https://img.shields.io/badge/Swagger-UI-85EA2D?logo=swagger)](https://swagger.io/)
[![AutoMapper](https://img.shields.io/badge/AutoMapper-9.0-FF6F00?logo=automapper&logoColor=white)](https://automapper.org/)
[![Serilog](https://img.shields.io/badge/Serilog-Logging-2B2E3A?logo=serilog&logoColor=white)](https://serilog.net/)
[![SendGrid](https://img.shields.io/badge/SendGrid-Email-21A7FF?logo=sendgrid&logoColor=white)](https://sendgrid.com/)
[![Health Check](https://img.shields.io/badge/Health_Check-Healthy-44cc11?logo=heartbeat&logoColor=white)](https://learn.microsoft.com/aspnet/core/host-and-deploy/health-checks)


[![Azure](https://img.shields.io/badge/Azure-Cloud-0078D4?logo=microsoft-azure&logoColor=white)](https://azure.microsoft.com/)
[![Azure AD B2C](https://img.shields.io/badge/Azure_AD_B2C-Identity-0078D4?logo=microsoft-azure&logoColor=white)](https://learn.microsoft.com/azure/active-directory-b2c/)
[![Azure SQL](https://img.shields.io/badge/Azure_SQL-Database-0078D4?logo=microsoft-azure&logoColor=white)](https://azure.microsoft.com/products/azure-sql/)
[![Azure Blob Storage](https://img.shields.io/badge/Azure_Blob_Storage-Storage-0078D4?logo=microsoft-azure&logoColor=white)](https://azure.microsoft.com/products/storage/blobs/)
[![Azure Functions](https://img.shields.io/badge/Azure_Functions-Serverless-0062AD?logo=azure-functions&logoColor=white)](https://azure.microsoft.com/products/functions/)
[![Azure DevOps](https://img.shields.io/badge/Azure_DevOps-CI/CD-0078D7?logo=azure-devops&logoColor=white)](https://azure.microsoft.com/services/devops/)
[![Azure Web App](https://img.shields.io/badge/Azure_Web_App-Hosting-0078D4?logo=microsoft-azure&logoColor=white)](https://azure.microsoft.com/products/app-service/)
[![Application Insights](https://img.shields.io/badge/Application_Insights-Monitoring-68217A?logo=microsoft-azure&logoColor=white)](https://azure.microsoft.com/products/monitor/)


# Project Overview

EduPlatform is a learning management system designed to simplify and enhance the online education experience 
for both students and instructors. Educators can easily create and manage courses, 
while students can browse, enroll in, and complete courses at their own pace or join live sessions. 
The platform also supports real-time interaction to promote active learning and engagement.

---


## Key Features

**User Management:** Secure user registration, authentication, and profile management using Azure AD B2C.

**Course Management:** Instructors can create courses with multiple sessions, including videos, documents, and other materials.

**Enrollment Management:** Students can enroll in courses, track progress, and receive certificates upon completion.

**Payment Integration:** Secure payment processing for paid courses using a payment gateway.

**Real-Time Communication:** SignalR-powered chat functionality for student-instructor interactions.

**Notifications:** Automated email notifications using SendGrid for course updates, payment confirmations, and reminders.

**Content Delivery:** Courses can be delivered as live sessions or pre-recorded videos stored in Azure Blob Storage.

**Monitoring and Insights:** Application performance and user behavior tracked using Application Insights.  

**Security:** Application secrets and sensitive data are securely stored in Azure Key Vault.  

**Scalability:** The application is hosted on Azure Web App with CI/CD pipelines managed by Azure DevOps.


---


### Application Workflow:

1. **Authentication & Authorization:**
   - User logs in via Azure AD (using AdObjId)
   - System reads UserRole to determine what they can access (Admin, Instructor, Student).
	
2. **Course Management (Instructor):**
   - Instructors create and manage courses.
   - Instructor can create/edit courses: select category, set price, seats, dates.
   - Instructor uploads sessions (video URLs) in order VideoOrder.

3. **Course Discovery (Student):**
   - Students can search for courses by keywords or browse by category.
   - CourseCategory is used to filter courses.
   - Students can view course details: description, price, instructor bio, sessions list.

4. **Enrollment & Payment:**
   - Student clicks “Enroll”; creates an Enrollment record with status “Pending.”
   - Payment form submits → Payment record is created with status “Pending”.
   - Payment processing is handled through a secure Payment gateway.
   - Enrollment status is updated based on payment confirmation.
   - On gateway callback, payment status updated to “Completed” or “Failed.”
   - Enrollment status is synchronized to match payment.

5. **Feedback Loop:**
   - Students can leave reviews and ratings for courses (1–5 stars + comments).
   - Instructors can view feedback and ratings to improve course material.

6. **Video Requests:**
   - Any user may fill out a VideoRequest form for new video content.
   - Instructors review requests, respond via the Response field, and attach produced video URLs.




### What Students Can Do:

- Register and log in using Azure AD B2C.

- Browse courses by category or search keywords.

- View course details: instructor bio, session outline, price, capacity.

- Enroll in courses (triggers enrollment/payment flow).

- Leave reviews and ratings after/during courses.

- Submit video requests for new topics.

- Receive notifications for course updates, payment confirmations, and reminders.


### What Instructors Can Do:

- Register and log in using Azure AD B2C.

- Maintain personal bio linked to their user profile.

- Create/Edit courses: title, type, price, dates, seat counts.

- Upload session content in order (video URL, titles, descriptions).

- Review student enrollments and payment statuses.

- Monitor reviews left on their courses.

- Fulfill video requests: fill in responses and attach new content links.



---


## Technologies Stack

**Frontend:** Angular 18  

**Backend:** .NET Core 9 with Entity Framework Core 9  

**Database:** Azure SQL Server  

**Authentication & Authorization:** Azure AD B2C  

**Cloud Storage:** Azure Storage Account(Azure Blob Storage) for media files  

**Caching:** In-Memory Cache  

**Logging:** Serilog 

**Monitoring:** Application Insights 

**Emails and Notifications:** SendGrid  

**Real-Time Communication:** SignalR  

**Deployment:** Azure Web App  

**Serverless Functions:** Azure Functions for specific tasks  

**Source Code Management:** Azure DevOps

**CI/CD:** Azure DevOps

**WorkFlow Automation:** Azure Logic Apps for automating workflows and integrating with other services.

**API Documentation:** Swagger

**Object Mapping:** AutoMapper

**Model Validation:** FluentValidation

**Payment Gateway:** Stripe



## Integrations with Other Services


- **Azure AD B2C:** For user authentication and authorization.
- **Azure SQL Server:** For relational database management and data storage.
- **Azure Blob Storage:** For storing media files and other static content.
- **SendGrid:** For sending email notifications when a video request is added or updated in VideoRequest table in database.
- **Stripe:** For secure payment processing.
- **Application Insights:** For monitoring application performance and user behavior.
- **Serilog:** For structured logging and diagnostics.
- **Azure Key Vault:** For securely storing application secrets and sensitive data.
- **Azure DevOps:** For source code management, CI/CD pipelines.
- **Azure Web App:** For hosting the application in a scalable and managed environment.
- **Azure Function App:** For running multiple Azure Functions For Background Jobs and serverless computing tasks.
- **Health Check:** For monitoring the health of the application and its dependencies and to check the Liveness and Readiness.