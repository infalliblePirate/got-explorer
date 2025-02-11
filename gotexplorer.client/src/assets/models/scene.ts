import * as THREE from 'three';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';

export class Scene {
  private scene: THREE.Scene;
  private camera: THREE.PerspectiveCamera;
  private renderer: THREE.WebGLRenderer;
  private controls: OrbitControls;
  private directionalLight!: THREE.DirectionalLight;
  private ambientLight!: THREE.AmbientLight;

  constructor(private container: HTMLElement) {
    this.scene = new THREE.Scene();
    this.camera = new THREE.PerspectiveCamera(
      50,
      container.clientWidth / container.clientHeight,
      0.1,
      1000
    );
    this.camera.position.z = 5;

    this.renderer = new THREE.WebGLRenderer();
    this.renderer.setSize(container.clientWidth, container.clientHeight);
    container.appendChild(this.renderer.domElement);

    this.controls = new OrbitControls(this.camera, this.renderer.domElement);
    this.controls.minDistance = 35;
    this.controls.maxDistance = 100;

    this.setupLight();


    window.addEventListener('resize', () => this.onWindowResize());
  }

  public setupLight() {
    this.ambientLight = new THREE.AmbientLight(0x404040, 1);
    this.scene.add(this.ambientLight);

    this.directionalLight = new THREE.DirectionalLight(0xffffff, 1);
    this.directionalLight.position.set(5, 5, 5);
    this.scene.add(this.directionalLight);
  }

  public removeDirectionalLight() {
    if (this.directionalLight) {
        this.scene.remove(this.directionalLight);
    }
  }

  private onWindowResize() {
    this.camera.aspect = this.container.clientWidth / this.container.clientHeight;
    this.camera.updateProjectionMatrix();
    this.renderer.setSize(this.container.clientWidth, this.container.clientHeight);
  }

  public loadModel(modelPath: string) {
    const loader = new GLTFLoader();
    loader.load(
      modelPath,
      (gltf: GLTFLoader) => {
        this.scene.add(gltf.scene);

        // center camera
        const box = new THREE.Box3().setFromObject(gltf.scene);
        const center = box.getCenter(new THREE.Vector3());
        const size = box.getSize(new THREE.Vector3());

        this.camera.position.set(center.x, center.y, center.z + size.length());
        this.camera.lookAt(center);
      },
      undefined,
      (error: Error) => {
        console.error('Error loading model:', error);
      }
    );
  }

  public loadBackground(backgroundPath: string) {
    const loader = new THREE.TextureLoader();
    const texture = loader.load(
      backgroundPath,
      () => {
        texture.mapping = THREE.EquirectangularReflectionMapping;
        texture.colorSpace = THREE.SRGBColorSpace;
        this.scene.background = texture;
      }
    );
  }

  public animate() {
    const render = () => {
      requestAnimationFrame(render);
      if (this.controls) this.controls.update();
      this.renderer.render(this.scene, this.camera);
    };
    render();
  }
}

// export function init() {
//   const container = document.getElementById('three');
//   if (!container) {
//     console.error('Cannot initialize, could not find the container element');
//     return;
//   }

//   const scene = new Scene(container);
//   scene.loadModel('assets/scene.glb');
//   scene.loadBackground('assets/onion.jpg');
//   scene.animate();
// }


