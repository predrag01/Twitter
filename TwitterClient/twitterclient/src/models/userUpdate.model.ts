export interface UserUpdate {
    id?: number;
    username?: string;
    name?: string;
    lastName?: string;
    email?: string;
    profilePicture?: string;
    changePass?: boolean;
    oldPass?: string;
    newPass?: string;
  }