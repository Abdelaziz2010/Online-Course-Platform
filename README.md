# EduPlatform {{Project Under Construction}}

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

**Logging:** Serilog and Application Insights  

**Notifications:** SendGrid  

**Real-Time Communication:** SignalR  

**Deployment:** Azure Web App  

**Serverless Functions:** Azure Functions for specific tasks  

**Monitoring:** Azure Monitor  

**Source Code Management:** Azure DevOps

**CI/CD:** Azure DevOps

**Containerization:** Docker for containerizing the application, ensuring consistency across different environments.

**WorkFlow Automation:** Azure Logic Apps for automating workflows and integrating with other services.

**API Documentation:** Swagger

**Object Mapping:** AutoMapper

**Model Validation:** FluentValidation

**Payment Gateway:** Stripe