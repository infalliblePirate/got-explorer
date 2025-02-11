interface Social {
  icon: string;
  link: string;
}

interface TeamMemberProps {
  name: string;
  role: string;
  description: string;
  image: string;
  socials: Social[];
}

const TeamMember: React.FC<TeamMemberProps> = ({ name, role, description, image, socials }) => {
  return (
    <div className="card">
      <div className="picture">
        <img src={image} alt={`${name}'s profile`} />
      </div>
      <p className="name">{name}</p>
      <p className="tech-role">{role}</p>
      <p className="description">{description}</p>
      <div className="linksOnSocials">
        {socials.map((social, index) => (
          <a
            key={index}
            href={social.link}
            target="_blank"
            rel="noopener noreferrer"
          >
            <i className={`fab fa-${social.icon}`}></i>
          </a>
        ))}
      </div>
    </div>
  );
};

export default TeamMember;