/* eslint-disable @typescript-eslint/no-explicit-any */
import Footer from '../additional_components/Footer';
import Navigation from '../additional_components/Navigation';
import './ProfilePage.scss';
import Cookies from 'universal-cookie';
import { Navigate, useNavigate } from 'react-router-dom';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { useEffect, useRef, useState } from 'react';
import authService from './authService';
import { toast } from 'sonner';
import GameService from '../games/GameService';
import { Player } from '../games/LeaderBoard';
import { useProfileImage } from "./ImageContex";
import ErrorHandle from '../../utils/ErrorHandle';

const Popover = ({ onSelectImage, popoverRef }: { onSelectImage: (imageId: string) => void; popoverRef: React.RefObject<HTMLDivElement>; }) => {
    const [children, setChildren] = useState<JSX.Element[]>([]);
    const getImages = async () => {
        const authserv = authService;
        const r = await authserv.get_images();
        r.forEach((item: any) => {
            if (item.name != "") {
                setChildren((prevChildren) => [...prevChildren, <PossiblePicture key={item.id} id={item.id} source={item.path} onSelect={onSelectImage} />]);

            }
        })
    }
    useEffect(() => {
        getImages();
    }, [])
    return (<div id="popover" className="popover" ref={popoverRef}>
        {children}
    </div>);
}
const PossiblePicture = ({ id, source, onSelect }: { id: string, source: string, onSelect: (id: string) => void }) => {
    const cookies = new Cookies();
    const handleMouseEvent = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        cookies.set("changedImage", (e.target as Element).id);
        onSelect(id);
    };

    return <button onClick={handleMouseEvent}>
        <img id={id} src={source} />
    </button>
}
const ProfilePage = () => {
    interface DecodedToken extends JwtPayload {
        name: string;
        email: string;
        Id: number | null;
    }
    const cookies = new Cookies();
    const token = cookies.get('token');
    const isAuthenticated = token != null ? true : false;
    const authserv = authService;
    const navigate = useNavigate();
    const [userData, setUserData] = useState<DecodedToken>({
        name: "",
        email: "",
        Id: null
    });
    const [changedName, setChangedName] = useState("");
    const [changedPasswords, setChangedPasswords] = useState({
        oldPassword: "",
        changedPassword: ""
    });
    const [imageSrc, setImageSrc] = useState<string | undefined>(undefined);
    const [popover, setPopover] = useState(false);
    const popoverRef = useRef<HTMLDivElement>(null);
    const [leaderboard, setLeaderboard] = useState<Player[]>([]);
    const [userRank, setUserRank] = useState<number | null>(null);
    const [userScore, setUserScore] = useState(0);
    const [dailyLeaderboard, setDailyLeaderboard] = useState<Player[]>([]);
    const gameserv = GameService;
    const { setProfileImage, refreshProfileImage } = useProfileImage();

    const handleClickOutside = (event: MouseEvent) => {
        if (popoverRef.current && !popoverRef.current.contains(event.target as Node)) {
            setPopover(false);
        }
    };

    useEffect(() => {
        document.addEventListener("mousedown", handleClickOutside);
        try {
            const decoded = jwtDecode<DecodedToken>(token);
            setUserData({
                name: decoded.name,
                email: decoded.email,
                Id: Number(decoded.Id)
            });
            gameserv.getLeaderboard()
                .then(response => {
                    const data: Player[] = response.data;
                    setLeaderboard(data);
                    const userIndex = data.findIndex((user: Player) => user.userId === Number(decoded.Id));
                    if (userIndex !== -1) {
                        setUserRank(userIndex + 1);
                        setUserScore(data[userIndex].score);
                    }
                })
                .catch(error => toast.error(`Error fetching leaderboard:${ErrorHandle(error.response.data.errors)}`));
            gameserv.getLeaderboardDaily()
                .then(response => {
                    const data: Player[] = response.data;
                    setDailyLeaderboard(data);
                })
                .catch(error => toast.error(`Error fetching daily leaderboard:${ErrorHandle(error.response.data.errors)}`));
        } catch (error: any) {
            toast.error(`Token decoding failed: ${error.response.data.errors}`);
        }
        const fetchImage = () => {
            try {
                if (!cookies.get("changedImage")) {
                    authserv.get_image(cookies.get("imageId")).
                        then((resp) => {
                            setImageSrc(resp);
                        });
                }
                else {
                    authserv.get_image(cookies.get("changedImage")).
                        then((resp) => {
                            setImageSrc(resp);
                        });
                }
            } catch (error: any) {
                toast.error(`Error fetching image: ${error.response.data.errors}`);
            }
        };

        fetchImage();
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
            const changed = cookies.get("changedImage");
            const current = cookies.get("imageId");

            if (changed && changed !== current) {
                authserv
                    .update_photo()
                    .then(() => {
                        refreshProfileImage();
                    })
                    .catch((error) => {
                        toast.error(`Failed to update photo: ${error.response.data.errors}`);;
                    });
            }
        };

    }, []);

    const handleImageSelect = (imageId: string) => {
        try {
            authserv.get_image(imageId).
                then((resp) => {
                    setImageSrc(resp);
                    setProfileImage(resp);
                });
        } catch (error: any) {
            toast.error(`Error fetching image: ${error.response.data.errors}`);
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setChangedPasswords({
            ...changedPasswords,
            [e.target.name]: value
        });
    };
    const handlePopout = () => {
        setPopover(!popover);
    }
    function changeName() {
        if (changedName != "") {
            authserv.update(changedName, userData.email)
                .catch((error) => {
                    toast.error(`Error changing name: ${error.response.data.errors}`);
                });
        }
    }
    function changePassword() {
        if (changedPasswords.oldPassword != "" && changedPasswords.changedPassword != "") {
            authserv.update_password(changedPasswords.oldPassword, changedPasswords.changedPassword).then(() => { navigate(0); })
                .catch((error) => {
                    toast.error(`Error changing password: ${error.response.data.errors}`);
                });
        }
    }
    function deleteProfile() {
        const deletePromise = new Promise<void>((resolve, reject) => {
            try {
                authserv.delete();
                setTimeout(() => {
                    resolve();
                }, 2000);
            } catch (error: any) {
                toast.error(`Error deleting profile: ${error.response.data.errors}`);
                reject();
            }
        });

        toast.promise(deletePromise, {
            loading: 'Deleting account...',
            success: 'Account successfully deleted!',
            error: 'Error deleting account',
        });

        deletePromise.then(() => navigate("/"));
    }
    function logOut() {
        authserv.logout();
        navigate("/");
    }
    return (<>{isAuthenticated ?

        <div className="profile-page">
            <Navigation />
            <div className="profile-container">
                <div className="email">
                    {userData ? (
                        <p>{userData.email}</p>
                    )
                        : (
                            <p>No email</p>
                        )
                    }
                </div>
                <div className="profile-header">
                    <div className="profile-picture-wrapper popover-container">
                        <img
                            className="profile-picture"
                            src={imageSrc}
                            alt="Profile"
                        />
                        {
                            popover &&
                            <Popover onSelectImage={handleImageSelect} popoverRef={popoverRef} />
                        }
                        <button className="change-picture-button" onClick={handlePopout}>📸</button>
                    </div>
                    <h2 className="username">{userData.name}</h2>
                </div>

                <div className="profile-leaderboard">
                    <h4>Leader board</h4>
                    <div className="leaderboard-container">
                        <div className="leaderboard-left">
                            <h5>Standard Leaderboard</h5>
                            {leaderboard.length > 0 && userRank !== null ? (
                                <>
                                    <p className={leaderboard[0].userId === userData.Id ? "you" : ""}>
                                        1. {leaderboard[0].userId === userData.Id ? "You" : leaderboard[0].username} - {leaderboard[0].score} points
                                    </p>
                                    {userRank !== 1 && (
                                        <>
                                            {userRank === 2 && (
                                                <p className="you">
                                                    2. You - {userScore} points
                                                </p>
                                            )}
                                            {userRank > 2 && (
                                                <>
                                                    <p>...</p>
                                                    <p className="you">
                                                        {userRank}. You - {userScore} points
                                                    </p>
                                                </>
                                            )}
                                        </>
                                    )}
                                </>
                            ) : (
                                <p className="no-play">You haven't played yet</p>
                            )}
                        </div>
                        <div className="divider"></div>
                        <div className="leaderboard-right">
                            <h5>Daily Game</h5>
                            {dailyLeaderboard.length > 0 ? (
                                <>
                                    {dailyLeaderboard.map((user, index) =>
                                        user.userId === userData.Id && (
                                            <p key={user.userId} className="you">
                                                {index + 1}. You - {user.score} points
                                            </p>
                                        )
                                    )}
                                </>
                            ) : (
                                <p className="no-play">You haven't played the daily game yet</p>
                            )}
                        </div>
                    </div>
                </div>



                <div className="profile-form">
                    <div className="form-group">
                        <label htmlFor="changedName">Name</label>
                        <input id="changedName" name="changedName" type="text" placeholder="Change your name" onChange={(e) => setChangedName(e.target.value)} autoComplete="off" readOnly
                            onFocus={(e) => e.target.removeAttribute('readOnly')} />
                        <button className="save-button" onClick={changeName}>Save new username</button>
                    </div>
                    <hr />
                    <div className="form-group">
                        <label htmlFor="oldPassword">Old Password</label>
                        <input id="oldPassword" name="oldPassword" type="password" placeholder="Enter old password" onChange={handleChange} autoComplete="off" readOnly
                            onFocus={(e) => e.target.removeAttribute('readOnly')} />
                        <label htmlFor="changedPassword">New Password</label>
                        <input id="changedPassword" name="changedPassword" type="password" placeholder="Enter new password" onChange={handleChange} autoComplete="off" readOnly
                            onFocus={(e) => e.target.removeAttribute('readOnly')} />
                        <button className="save-button" onClick={changePassword}>Save new password</button>
                    </div>
                    <div className="two-buttons">
                        <button className="logout-button" onClick={logOut}>Log out</button>
                        <button className="delete-button" onClick={deleteProfile}>Delete account</button>
                    </div>
                </div>
            </div>
            <Footer />
        </div>
        : <Navigate to="/login"></Navigate>
    }
    </>
    );
}
export default ProfilePage;
