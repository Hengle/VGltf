//
// Copyright (c) 2019- yutopp (yutopp@gmail.com)
//
// Distributed under the Boost Software License, Version 1.0. (See accompanying
// file LICENSE_1_0.txt or copy at  https://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VJson;
using VJson.Schema;

// Reference: https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/*
namespace VGltf.Types
{
    [JsonSchema(Id = "glTF.schema.json")]
    public class Gltf : GltfProperty
    {
        [JsonField(Name = "extensionsUsed"), JsonFieldIgnorable]
        [JsonSchema(UniqueItems = true, MinItems = 1)]
        public string[] ExtensionsUsed;

        [JsonField(Name = "extensionsRequired"), JsonFieldIgnorable]
        [JsonSchema(UniqueItems = true, MinItems = 1)]
        public string[] ExtensionsRequired;

        [JsonField(Name = "accessors"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<Accessor> Accessors;

        [JsonField(Name = "animations"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<Animation> Animations;

        [JsonField(Name = "asset")]
        [JsonSchemaRequired]
        public Asset Asset;

        [JsonField(Name = "buffers"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<Buffer> Buffers;

        [JsonField(Name = "bufferViews"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<BufferView> BufferViews;

        [JsonField(Name = "cameras"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<Camera> Cameras;

        [JsonField(Name = "images"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<Image> Images;

        [JsonField(Name = "materials"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<Material> Materials;

        [JsonField(Name = "meshes"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<Mesh> Meshes;

        [JsonField(Name = "nodes"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<Node> Nodes;

        [JsonField(Name = "samplers"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<Sampler> Samplers;

        [JsonField(Name = "scene"), JsonFieldIgnorable]
        [JsonSchemaDependencies("scenes"), JsonSchemaRef(typeof(GltfID))]
        public int? Scene;

        [JsonField(Name = "scenes"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<Scene> Scenes;

        [JsonField(Name = "skins"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<Skin> Skins;

        [JsonField(Name = "textures"), JsonFieldIgnorable]
        [JsonSchema(MinItems = 1)]
        public List<Texture> Textures;

        //

        /// <summary>
        ///   Returns indices of root nodes if the Scene is defined.
        ///   Returns empty IE<int> if the Scene is undefined or nodes are not defined in the scene.
        /// </summary>
        public IEnumerable<int> RootNodesIndices {
            get {
                if (Scene == null)
                {
                    return Enumerable.Empty<int>();
                }

                var node = Scenes[Scene.Value];
                if (node.Nodes == null) {
                    return Enumerable.Empty<int>();
                }

                return node.Nodes;
            }
        }

        /// <summary>
        ///   Returns root nodes if the Scene is defined.
        ///   Returns empty IE<Node> if the Scene is undefined or nodes are not defined in the scene.
        ///   Raise exceptions if any elements can not be found.
        /// </summary>
        public IEnumerable<Node> RootNodes {
            get {
                return RootNodesIndices.Select(i => Nodes[i]);
            }
        }
    }

    public static class GltfExtensions
    {
        /// <summary>
        ///   NOTE: Throw exceptions if elements are not found.
        /// </summary>
        public static Image GetImageByTextureIndex(this Gltf gltf, int index, out int? imageIndex)
        {
            var tex = gltf.Textures[index];
            imageIndex = tex.Source;

            return gltf.Images[imageIndex.Value];
        }

        public static Image GetImageByTextureIndex(this Gltf gltf, int index)
        {
            int? dummy;
            return GetImageByTextureIndex(gltf, index, out dummy);
        }

        public static Sampler GetSamplerByTextureIndex(this Gltf gltf, int index, out int? samplerIndex)
        {
            var tex = gltf.Textures[index];
            samplerIndex = tex.Sampler;

            return gltf.Samplers[samplerIndex.Value];
        }

        public static Sampler GetSamplerByTextureIndex(this Gltf gltf, int index)
        {
            int? dummy;
            return GetSamplerByTextureIndex(gltf, index, out dummy);
        }
    }
}
