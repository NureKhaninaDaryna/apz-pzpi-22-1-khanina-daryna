export interface User {
   email: string;
   token: string;
   role: UserRole;
}

export interface UserDto {
   email: string;
   role: UserRole;
}

export interface UserWithId {
   id: number;
   email: string;
   role: UserRole;
}

export interface UserFromValues {
   email: string;
   password: string;
}

export enum UserRole
{
   Admin,
   FacilityManager,
   Staff,
   Analyst,
   Viewer
}