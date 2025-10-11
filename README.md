## Features

- **User Authentication**: Secure user registration, login, and logout functionality
- **Public Content Access**: Browse and read articles without requiring authentication
- **Article Management**: Create, edit, and publish your own articles with ease
- **Interactive Commenting**: Engage with the community through article comments (authenticated users)
- **Markdown Support**:
    - Built-in markdown editor for rich text formatting
    - Upload your own markdown files for seamless content migration

**Users**

- Handles user authentication, profiles, and account management
- Integrates with ASP.NET Core Identity for security

**Articles**

- Stores blog posts with metadata (title, content, publish date, author)
- Supports markdown content with rich formatting
- Tracks view counts and engagement metrics

**Comments**

- Enables user interaction through article commenting
- Maintains threading and moderation capabilities
- Links to authenticated users and specific articles

**Categories**

- Provides article organization and filtering
- Supports hierarchical categorization

## Authentication System

Utilizes ASP.NET Core Identity for comprehensive user management, including registration, login, logout, and session handling.

## Markdown Processing

- **Editor Integration**: Real-time markdown editor with live preview
- **File Upload**: Support for importing existing markdown documents
- **Rendering Engine**: Server-side markdown to HTML conversion
- **Syntax Highlighting**: Code block support with language detection

## Article Management

- **CRUD Operations**: Full create, read, update, delete functionality
- **Draft System**: Save articles as drafts before publishing
- **Public Access**: Anonymous users can browse published content
- **Author Dashboard**: Manage personal articles and statistics

## Comment System

- **Threaded Comments**: Nested comment support for discussions
- **Moderation Tools**: Content management and spam prevention
- **User Interaction**: Like/dislike functionality for community engagement

## Authentication Endpoints

- User registration and login workflows
- Session management and logout functionality
- Password reset and account verification

## Content Management

- Article CRUD operations with authorization
- File upload handling for markdown documents
- Comment management with user permissions

## Public API

- Anonymous access to published articles
- Search and filtering capabilities
- Category-based content organization


this is the project that i think to build with use of this,

## Tech Stack

- **Backend Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core
- **Database**: Microsoft SQL Server
- **Authentication**: ASP.NET Core Identity
- **IDE**: Jet Brains Rider
- **Frontend**: Razor Pages with Bootstrap 5

read the whole description perfectly. and without changing model start the working on all the files.
i repeat don't change models.
