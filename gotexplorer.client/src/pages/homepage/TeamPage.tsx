import TeamMember from "./TeamMember";
import GOTlogo from "../../assets/images/GOTlogo.webp";
import uliana from "../../assets/images/team_members/uliana.webp";
import andrii from "../../assets/images/team_members/andrii.webp";
import katya from "../../assets/images/team_members/katya.webp";
import liza from "../../assets/images/team_members/liza.webp";
import dima from "../../assets/images/team_members/dima.webp";
import nastya from "../../assets/images/team_members/nastya.webp";
import marie from "../../assets/images/team_members/marie.webp";


const TeamPage = () => {
    return (
        <div className="team-page">
            <div className="team-page__content">
                <header className="team-page__header">
                    <img src={GOTlogo} alt="GOTExplorer Logo" />
                    <h2>Team</h2>
                </header>
                <div className="team-page__grid">
                    <TeamMember
                        name="Uliana Yeshchenko"
                        role="PM, Frontend developer, UI/UX designer"
                        description="There are many variations of passages of Lorem Ipsum available"
                        image={uliana}
                        socials={[
                            { icon: "facebook", link: "https://facebook.com" },
                            { icon: "instagram", link: "https://instagram.com" },
                            { icon: "twitter", link: "https://twitter.com" },
                        ]}
                    />
                    <TeamMember
                        name="Andrii Avdiiuk"
                        role="Backend developer"
                        description="There are many variations of passages of Lorem Ipsum available"
                        image={andrii}
                        socials={[
                            { icon: "facebook", link: "https://facebook.com" },
                            { icon: "instagram", link: "https://instagram.com" },
                            { icon: "twitter", link: "https://twitter.com" },
                        ]}
                    />

                    <TeamMember
                        name="Kateryna Pavlenko"
                        role="Backend developer"
                        description="There are many variations of passages of Lorem Ipsum available"
                        image={katya}
                        socials={[
                            { icon: "facebook", link: "https://facebook.com" },
                            { icon: "instagram", link: "https://instagram.com" },
                            { icon: "twitter", link: "https://twitter.com" },
                        ]}
                    />
                    <TeamMember
                        name="Yelyzaveta Bredikhina"
                        role="Frontend developer"
                        description="There are many variations of passages of Lorem Ipsum available"
                        image={liza}
                        socials={[
                            { icon: "facebook", link: "https://facebook.com" },
                            { icon: "instagram", link: "https://instagram.com" },
                            { icon: "twitter", link: "https://twitter.com" },
                        ]}
                    />
                    <TeamMember
                        name="Dmytro Levkovych"
                        role="QA"
                        description="There are many variations of passages of Lorem Ipsum available"
                        image={dima}
                        socials={[
                            { icon: "facebook", link: "https://facebook.com" },
                            { icon: "instagram", link: "https://instagram.com" },
                            { icon: "twitter", link: "https://twitter.com" },
                        ]}
                    />
                    <TeamMember
                        name="Anastasia Grinke"
                        role="3D artist, Fronted developer, UI/UX designer"
                        description="There are many variations of passages of Lorem Ipsum available"
                        image={nastya}
                        socials={[
                            { icon: "facebook", link: "https://facebook.com" },
                            { icon: "instagram", link: "https://instagram.com" },
                            { icon: "twitter", link: "https://twitter.com" },
                        ]}
                    />
                    <TeamMember
                        name="Mariia Yakovenko"
                        role="3D artist, UI/UX designer"
                        description="There are many variations of passages of Lorem Ipsum available"
                        image={marie}
                        socials={[
                            { icon: "facebook", link: "https://facebook.com" },
                            { icon: "instagram", link: "https://instagram.com" },
                            { icon: "twitter", link: "https://twitter.com" },
                        ]}
                    />
                </div>
            </div>
        </div>
    );
};

export default TeamPage;