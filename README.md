# Ordering API

The Ordering API interacts with the shopping app and the catalog API to manage cart, ordering, and user functionalities.

## Features

### Technologies Used

- .NET 8
- Web API
- EF Core
- In-Memory Database (for cost-effective hosting)

### Architecture

- **Onion Architecture**
  - Layers: Application, Infrastructure, Domain (with an anemic domain model)
- **Vertical Slice Architecture**
  - Grouped by major features, with some liberties taken where necessary
- **Application Layer**
  - General structure: Endpoint, Service, Repository
  - Minimal APIs
  - Good use of extensions for configuration and middleware

### Functionality

- **Comprehensive User-Account Functionality:**
  - User Accounts: Manage addresses, edit profile, 2FA login, recovery codes, password reset, account validation, session management, orders view
- **Fully Asynchronous Project:** Ensures efficient and non-blocking operations.
- **Authentication:**
  - JWT and Cookie Authentication: Supports users who disable cookies by using short tokens.
  - Cookie Sessions: Stored in the database, allowing users to log out remotely.
- **Reply Pattern:** Uses a semi-functional immutable data type for return values, handling nullables gracefully and allowing error messages to bubble up.
- **Error Handling:** Comprehensive error handling with both low-level and global exception catching in middleware and graceful handling using the reply pattern.
- **Configuration and Environment Variables:** Proper usage throughout the project.

### Endpoints

- **Account Management:**
  - `api/account/addresses/view`
  - `api/account/addresses/add`
  - `api/account/addresses/update`
  - `api/account/addresses/delete`
  - `api/account/delete`
  - `api/account/orders/view`
  - `api/account/orders/details`
  - `api/account/profile/view`
  - `api/account/profile/update`
  - `api/account/register/resendConfirmEmail`
  - `api/account/register/confirmEmail`
  - `api/account/register`
  - `api/account/security/view`
  - `api/account/security/updatePassword`
  - `api/account/security/generateRecovery`
  - `api/account/security/disable2fa`
  - `api/account/security/update2fa`
  - `api/account/sessions/view`
  - `api/account/sessions/delete`

- **Authentication:**
  - `api/authentication/login`
  - `api/authentication/2fa`
  - `api/authentication/recover`
  - `api/authentication/refresh`
  - `api/authentication/forgot`
  - `api/authentication/reset`
  - `api/authentication/logout`
