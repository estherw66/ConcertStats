export type User = {
    id: number;
    username: string;
    email: string;
    lastLoginAt: Date;
    profileFullName: string;
    profilePictureUrl: string;
    profileBio: string;
    profileLocation: string;
    languageSettings: string;
    themeSettings: string;
    privacySettings: string;
}

export type CreateUserRequest = {
    username: string;
    fullName: string;
    email: string;
    password: string;
}