# EduPlatform {{Project Under Construction}}

# Project Overview

EduPlatform is a learning management system designed to simplify and enhance the online education experience 
for both students and instructors. Educators can easily create and manage courses, 
while students can browse, enroll in, and complete courses at their own pace or join live sessions. 
The platform also supports real-time interaction to promote active learning and engagement.

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


## Technologies Stack

**Frontend:** Angular 18  

**Backend:** .NET Core 9 with Entity Framework Core 9  

**Database:** Azure SQL Server  

**Authentication:** Azure AD B2C  

**Cloud Storage:** Azure Storage Account(Azure Blob Storage) for media files  

**Caching:** In-Memory Cache  

**Logging:** Serilog and Application Insights  

**Notifications:** SendGrid  

**Real-Time Communication:** SignalR  

**Deployment:** Azure Web App  

**Serverless Functions:** Azure Functions for specific tasks  

**Monitoring:** Azure Monitor  

**Source Code Management:** Azure DevOps

**CI/CD:** Azure DevOps

**API Documentation:** Swagger

**Object Mapping:** AutoMapper

**Model Validation:** FluentValidation

**Payment Processing:** Stripe