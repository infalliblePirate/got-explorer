import TeamMember from "./TeamMember";
import GOTlogo from "../../assets/images/GOTlogo.png"; 


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
            name="Leo Arcand"
            role="Software Engineer"
            description="There are many variations of passages of Lorem Ipsum available"
            image="https://via.placeholder.com/150"
            socials={[
              { icon: "facebook", link: "https://facebook.com" },
              { icon: "instagram", link: "https://instagram.com" },
              { icon: "twitter", link: "https://twitter.com" },
            ]}
          />
          <TeamMember
            name="Leo Arcand"
            role="Software Engineer"
            description="There are many variations of passages of Lorem Ipsum available"
            image="https://via.placeholder.com/150"
            socials={[
              { icon: "facebook", link: "https://facebook.com" },
              { icon: "instagram", link: "https://instagram.com" },
              { icon: "twitter", link: "https://twitter.com" },
            ]}
          />
    
          <TeamMember
            name="Leo Arcand"
            role="Software Engineer"
            description="There are many variations of passages of Lorem Ipsum available"
            image="https://via.placeholder.com/150"
            socials={[
              { icon: "facebook", link: "https://facebook.com" },
              { icon: "instagram", link: "https://instagram.com" },
              { icon: "twitter", link: "https://twitter.com" },
            ]}
          />
          <TeamMember
            name="Leo Arcand"
            role="Software Engineer"
            description="There are many variations of passages of Lorem Ipsum available"
            image="https://via.placeholder.com/150"
            socials={[
              { icon: "facebook", link: "https://facebook.com" },
              { icon: "instagram", link: "https://instagram.com" },
              { icon: "twitter", link: "https://twitter.com" },
            ]}
          />
          <TeamMember
            name="Leo Arcand"
            role="Software Engineer"
            description="There are many variations of passages of Lorem Ipsum available"
            image="https://via.placeholder.com/150"
            socials={[
              { icon: "facebook", link: "https://facebook.com" },
              { icon: "instagram", link: "https://instagram.com" },
              { icon: "twitter", link: "https://twitter.com" },
            ]}
          />
          <TeamMember
            name="Leo Arcand"
            role="Software Engineer"
            description="There are many variations of passages of Lorem Ipsum available"
            image="https://via.placeholder.com/150"
            socials={[
              { icon: "facebook", link: "https://facebook.com" },
              { icon: "instagram", link: "https://instagram.com" },
              { icon: "twitter", link: "https://twitter.com" },
            ]}
          />
          <TeamMember
            name="Leo Arcand"
            role="Software Engineer"
            description="There are many variations of passages of Lorem Ipsum available"
            image="https://via.placeholder.com/150"
            socials={[
              { icon: "facebook", link: "https://facebook.com" },
              { icon: "instagram", link: "https://instagram.com" },
              { icon: "twitter", link: "https://twitter.com" },
            ]}
          />
          <TeamMember
            name="Leo Arcand"
            role="Software Engineer"
            description="There are many variations of passages of Lorem Ipsum available"
            image="https://via.placeholder.com/150"
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