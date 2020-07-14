## Overview
Collaborator provides two approaches to interact with the server by API.

 1. Command-line client and GUI Client
     This approach does not consume a license. It provides most of commands to edit a review, add comments or defects, change the review stage and etc. But it is lack of retrieving the detailed review information.
 2. Json Web API
     This approach consumes a license. Compare to the Command-line client, it has ability to retrieve the detailed review information.
     
![Architect Diagram](Doc/Architect%20Diagram.png)

## Appendix ##
### Collaborator floating-seat licenses
SmartBear offers both fixed-seat and floating-seat licenses for Collaborator.

Floating-seat  licenses specify the maximum allowed number of active  _concurrent_  users. A user is considered active if they are currently consuming a license. Floating-seat licenses are most appropriate when you have many users that will work with Collaborator occasionally.

**License Consumption**

The below describes how  **floating-seat license**  are consumed.

***Collaborator WebUI***

-   When a floating-seat user logs in to Collaborator via a web browser, a license is assigned to that individual.
-   The license will remain assigned to the user for as long as there is a Collaborator tab open in the browser.
-   The license will be returned to the pool once all Collaborator tabs are closed, and one hour has passed.
-   The license will be returned to the pool immediately if the user clicks “Logout”.

***Collaborator Command-Line Client and GUI Client***

-   The command-line client only consumes a license when using the  [`ccollab admin wget`](https://support.smartbear.com/collaborator/docs/reference/command-line/ccollab-admin-wget.html)  command.
-   The GUI client never consumes a license.
-   The tray notifier never consumes a license.

***Note***
The [system administrator](https://support.smartbear.com/collaborator/docs/server/settings/users.html#system-administrator) is always allowed to log in, even if the license limit has been reached.
