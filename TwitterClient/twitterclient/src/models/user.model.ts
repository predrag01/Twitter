export interface User {
    id?: number;
    username?: string;
    name?: string;
    lastName?: string;
    email?: string;
    profilePicture?: string;
    followingCount?: number;
    followersCount?: number;
    checkFollowing?: boolean;
  }