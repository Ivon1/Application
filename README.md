# CoWorking

A full-stack web application for booking coworking spaces with an intuitive interface and comprehensive booking management features. The project consists of a .NET Core Web API backend and an Angular frontend with responsive design for all devices. The backend follows a 3-layer architectural pattern ensuring clean separation of concerns and maintainability.

## Project Structure

The project is organized into two main folders:
- **BackendCoworking**: .NET Core Web API project
- **FrontendCoworking**: Angular application

## Application Screenshots

### Coworking Spaces Overview
![Coworking Spaces Page](https://github.com/Ivon1/Application/blob/develop/screenshots/coworking-spaces.png?raw=true)
*Browse available coworking spaces with location and availability information*

### Workspace Selection
![Workspace Selection](https://github.com/Ivon1/Application/blob/develop/screenshots/workspace-selection.png?raw=true)
*Choose from different workspace types including meeting rooms, private rooms, and open spaces*

### Booking Form
![New Booking Form](https://github.com/Ivon1/Application/blob/develop/screenshots/booking-form.png?raw=true)
*Create a new booking with date/time selection and workspace preferences*

![Edit Booking Form](https://github.com/Ivon1/Application/blob/develop/screenshots/edit-booking-form.png?raw=true)
*Edit existing bookings with pre-filled information*

### My Bookings
![Bookings Overview](https://github.com/Ivon1/Application/blob/develop/screenshots/current-bookings.png?raw=true)
*View all your current and upcoming bookings*

![Empty Bookings State](https://github.com/Ivon1/Application/blob/develop/screenshots/empty-bookings.png?raw=true)
*User-friendly empty state when no bookings exist*

### AI Assistant
![AI Assistant Interface](https://github.com/Ivon1/Application/blob/develop/screenshots/integrated-ai-assistant.png?raw=true)
*Get help with booking-related questions using the integrated AI assistant*

### Booking Management
![Booking Actions](https://github.com/Ivon1/Application/blob/develop/screenshots/confirmation-modal-to-delete.png?raw=true)
*Edit or cancel existing bookings with confirmation modals*

![Booking Success Modal](https://github.com/Ivon1/Application/blob/develop/screenshots/successful-booking-creation.png?raw=true)
*Confirmation modal after successful booking creation*

![Booking Error Modal](https://github.com/Ivon1/Application/blob/develop/screenshots/conflicts-with-booking.png?raw=true)
*Error handling for booking conflicts or issues*


### Setting up the Backend

1. Navigate to the `BackendCoworking` directory
2. Create a `secrets.json` file with your database connection string and Groq API key:
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=yourdbname;Username=yourusername;Password=yourpassword"
   dotnet user-secrets set "GroqApiKey" "yourApiKey"
   ```

3. Start the backend server:
   ```bash
   dotnet run
   ```
4. Open your browser and navigate to https://localhost:7201/swagger

### Setting up the FrontEnd
1. Navigate to the `FrontendCoworking` directory
2. Install dependencies:
   ```bash
   npm install
   ```
3. Configure the API URL:
   - Update the `apiUrl` in [`src/environments/environment.ts`](src/environments/environment.ts) and [`src/environments/environment.development.ts`](src/environments/environment.development.ts) to match your backend API URL
   - Default is set to `https://localhost:51842` (update this to your backend server URL)
4. Start the development server: 
   ```bash
   ng serve
   ```
5. Open your browser and navigate to http://localhost:4200

## Key Features

- **Responsive Design**: Works seamlessly on desktop and mobile devices
- **Real-time Availability**: Check workspace availability with date/time selection
- **AI-Powered Assistant**: Get help with booking queries using integrated AI
- **Booking Management**: Create, edit, and cancel bookings with ease
- **Multiple Workspace Types**: Support for meeting rooms, private rooms, and open spaces
- **Rich UI Components**: Custom date pickers, time selectors, and image galleries

- **Form Validation**: Comprehensive form validation for booking details




