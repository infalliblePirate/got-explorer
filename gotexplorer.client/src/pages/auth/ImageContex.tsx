import React, { createContext, useContext, useState, useEffect } from "react";
import Cookies from "universal-cookie";
import authService from "./authService"

type ProfileImageContextType = {
  profileImage: string | null;
  setProfileImage: (src: string) => void;
  refreshProfileImage: () => void;
};

const ProfileImageContext = createContext<ProfileImageContextType>({
  profileImage: null,
  setProfileImage: () => {},
  refreshProfileImage: () => {},
});

export const useProfileImage = () => useContext(ProfileImageContext);

export const ProfileImageProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const cookies = new Cookies();
  const [profileImage, setProfileImage] = useState<string | null>(null);

  const refreshProfileImage = async () => {
    const imageId = cookies.get("imageId");
    if (imageId) {
      try {
        const image = await authService.get_image(imageId);
        setProfileImage(image);
      } catch (error) {
        console.error("Failed to fetch profile image:", error);
      }
    }
  };

  useEffect(() => {
    refreshProfileImage();
  }, []);

  return (
    <ProfileImageContext.Provider
      value={{ profileImage, setProfileImage, refreshProfileImage }}
    >
      {children}
    </ProfileImageContext.Provider>
  );
};
